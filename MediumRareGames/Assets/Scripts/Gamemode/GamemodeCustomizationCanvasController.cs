using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GamemodeCustomization))]
public class GamemodeCustomizationCanvasController : MonoBehaviour
{
    [SerializeField] private GamemodeCustomization m_gamemodeCustomization;

    [SerializeField] private Dropdown m_roundCount;
    [SerializeField] private Dropdown m_weapon;

    [SerializeField] private Dropdown m_lmsLives;
    [SerializeField] private Dropdown m_lmsRespawnTimes;
    [SerializeField] private Dropdown m_lmsTimeLimits;

    [SerializeField] private Dropdown m_kothPointsCapture;
    [SerializeField] private Dropdown m_kothPointsPossible;
    [SerializeField] private Dropdown m_kothRespawnTimes;
    [SerializeField] private Dropdown m_kothCountdownTimes;

    [SerializeField] private Settings.KingOfTheHill m_koth;
    [SerializeField] private Settings.LastManStanding m_lms;
    [SerializeField] private Settings.Match m_match;
    [SerializeField] private Settings.GeneralWeapon m_genWeapon;

    // Use this for initialization
    private void Start()
    {
        if (m_gamemodeCustomization == null)
            m_gamemodeCustomization = gameObject.GetComponent<GamemodeCustomization>();

        // Initialize Gamemode Customization
        m_gamemodeCustomization.Initialized += Initialize;
        m_gamemodeCustomization.Initialize();
    }

    private void SetDropdown(Dropdown _Dropdown, string _Label)
    {
        if (_Dropdown == null)
            return;
        for (int i = 0; i < _Dropdown.options.Count; i++)
        {
            if (_Dropdown.options[i].text == _Label)
            {
                _Dropdown.value = i;
                break;
            }
        }
    }

    private void SetWeapon()
    {
        if (m_genWeapon == null || m_weapon == null)
            return;
        // Name the dropdown options so that they always line up with Weapon hiearchy
        for (int i = 0; i < m_weapon.options.Count; i++)
        {
            switch (i)
            {
                case (int)Weapon.Type.GumballLauncher:
                    m_weapon.options[i].text = "Gumball Launcher";
                    break;
                case (int)Weapon.Type.SharkZooka:
                    m_weapon.options[i].text = "SharkZooka";
                    break;
                case (int)Weapon.Type.WallGun:
                    m_weapon.options[i].text = "Wall-in-a-Can";
                    break;
                default:
                    m_weapon.options[i].text = "NULL";
                    break;
            }
        }
        m_weapon.value = (int)m_genWeapon.DefaultWeapon;
    }

    private void Initialize(object _Sender, EventArgs _Args)
    {
        SetDropdown(m_roundCount, m_match.RoundsToWin.ToString());
        SetWeapon();

        SetDropdown(m_lmsLives, m_lms.Lives.ToString());
        SetDropdown(m_lmsRespawnTimes, m_lms.RespawnTime.ToString());
        SetDropdown(m_lmsTimeLimits, m_lms.TimeLimit.ToString());

        SetDropdown(m_kothPointsCapture, m_koth.PointsToCapture.ToString());
        SetDropdown(m_kothPointsPossible, m_koth.PossiblePoints.ToString());
        SetDropdown(m_kothRespawnTimes, m_koth.RespawnTime.ToString());
        SetDropdown(m_kothCountdownTimes, m_koth.CountdownTime.ToString());
    }
}
