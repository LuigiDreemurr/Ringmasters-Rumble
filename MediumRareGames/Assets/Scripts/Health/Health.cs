/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Health
       - An advanced health script that fires off certain events when changes are
         made

   Details:
       - Contains functionality allowing incoming damage/restore to be scaled
         using modifiers
       - Events when: Change, Damage, Restore, Death
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    public class Health : MonoBehaviour
    {
        #region Data Members
        [SerializeField] private float m_maxHealth = 100; //The max health
        [SerializeField] private float m_curHealth = 100; //The current health
        [Space]
        [SerializeField] private float m_constantDamageModifier = 1.0f;  //Constant value for scaling incoming damage
        [SerializeField] private float m_constantRestoreModifier = 1.0f; //Constant value for scaling incoming restore

        private float m_damageModifier = 1.0f; //Value for scaling incoming damage
        private float m_restoreModifier = 1.0f; //Value for scaling incoming damage

        #region Properties
        /// <summary>Returns the max health</summary>
        public float MaxHealth { get { return m_maxHealth; } }

        /// <summary>Returns the current health</summary>
        public float CurHealth { get { return m_curHealth; } }

        /// <summary>Returns the current health percent</summary>
        public float Percent { get { return m_curHealth / m_maxHealth; } }

        /// <summary>Returns whether or not the current health has reached zero</summary>
        public bool IsDead { get { return m_curHealth == 0; } }

        /// <summary>Returns the constant damage modifier</summary>
        public float ConstantDamageModifier { get { return m_constantDamageModifier; } }

        /// <summary>Returns the constant restore modifier</summary>
        public float ConstantRestoreModifier { get { return m_constantRestoreModifier; } }

        /// <summary>Value for scaling incoming damage</summary>
        public float DamageModifier
        {
            get { return m_damageModifier; }
            set { m_damageModifier = value; }
        }

        /// <summary>Value for scaling incoming restore</summary>
        public float RestoreModifier
        {
            get { return m_restoreModifier; }
            set { m_restoreModifier = value; }
        }
        
        /// <summary>Returns the combined constant and regular damage modifier</summary>
        public float TotalDamageModifier { get { return m_constantDamageModifier * m_damageModifier; } }

        /// <summary>Returns the combined constant and regular restore modifier</summary>
        public float TotalRestoreModifier { get { return m_constantRestoreModifier * m_restoreModifier; } }
        #endregion

        #endregion

        #region Events

        #region Delegate Signatures
        /// <summary>Delegate representing events involving a health change</summary>
        /// <param name="_Health"></param>
        public delegate void ChangeEvent(Health _Health);

        /// <summary>Delegate representing events involving damage</summary>
        /// <param name="_Health">The Health damaged</param>
        /// <param name="_Source">The DamageSource</param>
        public delegate void DamageEvent(Health _Health, DamageSource _Source);

        /// <summary>Delegate representing events involving restore</summary>
        /// <param name="_Health">The Health restored</param>
        /// <param name="_Source">The RestoreSource</param>
        public delegate void RestoreEvent(Health _Health, RestoreSource _Source);
        #endregion

        /// <summary>Event invoked when health is changed in any way</summary>
        public event ChangeEvent OnChange;

        /// <summary>Event invoked when health is damaged</summary>
        public event DamageEvent OnDamage;

        /// <summary>Event invoked when health is damaged, dropping to zero or below</summary>
        public event DamageEvent OnDeath;

        /// <summary>Event invoked when health is restored</summary>
        public event RestoreEvent OnRestore;

        #endregion

        #region Private Methods
        /// <summary>Clamps the current health from 0 to max health</summary>
        private void ClampHealth()
        {
            m_curHealth = Mathf.Clamp(m_curHealth, 0, m_maxHealth);
        }

        #region Modifier Changers
        /// <summary>Delegate to represent functions changing a modifier</summary>
        /// <param name="_Modifier">The modifier changed</param>
        /// <param name="_Change">The change applied to the modifier</param>
        private delegate void ChangeModifier(ref float _Modifier, float _Change);

        /// <summary>Modifier += Change</summary>
        static private void ModifierAdd(ref float _Modifier, float _Change)
        {
            _Modifier += _Change;
        }

        /// <summary>Modifier -= Change</summary>
        static private void ModifierSub(ref float _Modifier, float _Change)
        {
            _Modifier -= _Change;
        }

        /// <summary>Modifier*= Change</summary>
        static private void ModifierMult(ref float _Modifier, float _Change)
        {
            _Modifier *= _Change;
        }

        /// <summary>Modifier /= Change</summary>
        static private void ModifierDiv(ref float _Modifier, float _Change)
        {
            _Modifier /= _Change;
        }

        /// <summary>Changes the damage modifier for a duration</summary>
        /// <param name="_Value">The value applied to the modifier</param>
        /// <param name="_Duration">How long the change lasts</param>
        /// <param name="_Change">Function that changes the modifier</param>
        /// <param name="_Revert">Function that reverts the modifier</param>
        private IEnumerator ChangeDamageModifierRoutine(float _Value, float _Duration, ChangeModifier _Change, ChangeModifier _Revert)
        {
            _Change(ref m_damageModifier, _Value);

            yield return new WaitForSeconds(_Duration);

            _Revert(ref m_damageModifier, _Value);
        }

        /// <summary>Changes the restore modifier for a duration</summary>
        /// <param name="_Value">The value applied to the modifier</param>
        /// <param name="_Duration">How long the change lasts</param>
        /// <param name="_Change">Function that changes the modifier</param>
        /// <param name="_Revert">Function that reverts the modifier</param>
        private IEnumerator ChangeRestoreModifierRoutine(float _Value, float _Duration, ChangeModifier _Change, ChangeModifier _Revert)
        {
            _Change(ref m_restoreModifier, _Value);

            yield return new WaitForSeconds(_Duration);

            _Revert(ref m_restoreModifier, _Value);
        }
        #endregion

        #endregion

        #region Public Methods

        #region Modifier Routine Wrappers
        /// <summary>Scales the damage modifier for a given duration</summary>
        /// <param name="_Scale">What to scale the modifier with</param>
        /// <param name="_Duration">How long the modifier will stay scaled</param>
        public void ScaleDamageModifier(float _Scale, float _Duration)
        {
            StartCoroutine(ChangeDamageModifierRoutine(_Scale, _Duration, ModifierMult, ModifierDiv));
        }

        /// <summary>Scales the restore modifier for a given duration</summary>
        /// <param name="_Scale">What to scale the modifier with</param>
        /// <param name="_Duration">How long the modifier will stay scaled</param>
        public void ScaleRestoreModifier(float _Scale, float _Duration)
        {
            StartCoroutine(ChangeRestoreModifierRoutine(_Scale, _Duration, ModifierMult, ModifierDiv));
        }

        /// <summary>Scales the damage modifier for a given duration</summary>
        /// <param name="_Add">What to add to the modifier</param>
        /// <param name="_Duration">How long the modifier will stay changed</param>
        public void AddDamageModifier(float _Add, float _Duration)
        {
            StartCoroutine(ChangeDamageModifierRoutine(_Add, _Duration, ModifierAdd, ModifierSub));
        }

        /// <summary>Scales the restore modifier for a given duration</summary>
        /// <param name="_Add">What to add to the modifier</param>
        /// <param name="_Duration">How long the modifier will stay changed</param>
        public void AddRestoreModifier(float _Add, float _Duration)
        {
            StartCoroutine(ChangeRestoreModifierRoutine(_Add, _Duration, ModifierAdd, ModifierSub));
        }
        #endregion

        /// <summary>Damage the current health</summary>
        /// <param name="_Source">The incoming damage</param>
        public void Damage(DamageSource _Source)
        {
            //Apply modifier
            _Source.Damage *= TotalDamageModifier;

            //Damage
            m_curHealth -= _Source.Damage;
            ClampHealth();

            #region Invoke Events
            //Change event
                OnChange?.Invoke(this);

            //Damage event
                OnDamage?.Invoke(this, _Source);

            //Death event (if dead)
            if (IsDead)
            {
                if (gameObject.tag == "Player") // Needed to stop the announcer from mouthing off after every fence and barrel breaks
                    GameObject.FindGameObjectWithTag("Announcer")?.GetComponent<AnnouncerChatter>()?.Play_Player_Dead();
                OnDeath?.Invoke(this, _Source);
            }
            else
                GameObject.FindGameObjectWithTag("Announcer")?.GetComponent<AnnouncerChatter>()?.Play_Player_Hurt();
            #endregion
        }

        /// <summary>Restore the current health</summary>
        /// <param name="_Source">The incoming restore</param>
        public void Restore(RestoreSource _Source)
        {
            //Apply modifier
            _Source.Health *= TotalRestoreModifier;

            //Restore
            m_curHealth += _Source.Health;
            ClampHealth();

            #region Invoke Events
            //Change event
            OnChange?.Invoke(this);

            //Restore event
            OnRestore?.Invoke(this, _Source);
            #endregion
        }

        /// <summary>Resets the current health, the damage modifier, and the restore modifier</summary>
        public void Reset()
        {
            m_curHealth = m_maxHealth;
            m_damageModifier = 1.0f;
            m_restoreModifier = 1.0f;
            //Change event
            OnChange?.Invoke(this);
        }
        #endregion
    }
}
