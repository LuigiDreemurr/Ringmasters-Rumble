using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInput;

public class PlayerController : MonoBehaviour
{
    private Settings.Player m_settings;
    private XInput.Controller m_input;
    private Rigidbody m_rigidbody;
    private PlayerContainer m_container;

    public PlayerContainer Container
    {
        get { return m_container; }
        set { m_container = value; }
    }

    public XInput.Controller Input
    {
        get { return m_input; }
        set { m_input = value; }
    }

    private Vector3 m_moveDir = Vector3.zero;

    /// <summary>Initialization</summary>
    private void Start()
    {
        m_settings = GlobalSettings.Get.Player;
        m_rigidbody = GetComponent<Rigidbody>();

        //m_input.Subscribe(Button.Start, Pause);
        m_input.Subscribe(Button.LeftBumper, Discard);

        //Single axis events
        m_input.Subscribe(Axis.RightTrigger, Fire);

        //Dual axis events
        m_input.Subscribe(DualAxis.LeftStick, Move);
        m_input.Subscribe(DualAxis.RightStick, Aim);
    }

    private void Discard(XInput.Controller _Contorller, ButtonArgs _Args)
    {
        //Ignore button up/held
        if (_Args.Action != ButtonAction.Down || !transform.parent.gameObject.activeSelf)
            return;

        GetComponent<Weapon.Carrier>().DiscardWeapon();
    }

    private void Fire(XInput.Controller _Contorller, AxisArgs<float> _Args)
    {
        //Don't bother with dead values
        if (_Args.CurrState <= 0 || !transform.parent.gameObject.activeSelf)
            return;

        GetComponent<Weapon.Carrier>().Weapon.Fire();
    }

    /// <summary>Move the player based on input from a dual axis (LeftStick)</summary>
    /// <param name="_Sender">Where the event is coming from</param>
    /// <param name="_Args">The event arguments</param>
    private void Move(XInput.Controller _Contorller, AxisArgs<Vector2> _Args)
    {
        if (!transform.parent.gameObject.activeSelf)
            return;

        //Calculate the moving direction from the joystick axes
        m_moveDir = new Vector3(_Args.CurrState.x, 0, _Args.CurrState.y).normalized;

        float gravity = m_rigidbody.velocity.y; //Get the 'gravity' before overriding velocity

        if(GetComponent<PlayerFalling>().isFalling)
        {

            m_rigidbody.velocity = m_moveDir * (m_settings.MoveSpeed/3);
        }
        else
        {

            m_rigidbody.velocity = m_moveDir * m_settings.MoveSpeed;
        }
        //Set the velocity y to gravity (don't want it to scale with move speed)
        Vector3 newVel = m_rigidbody.velocity;
        newVel.y = gravity;
        m_rigidbody.velocity = newVel;

        //Handle animation
        Animator animator = GetComponent<Animator>();

        Vector3 angleAdjust = Quaternion.AngleAxis(-transform.eulerAngles.y, Vector3.up) * m_moveDir;
        animator.SetFloat("Horizontal", angleAdjust.x);
        animator.SetFloat("Vertical", angleAdjust.z);
    }

    /// <summary>Aim the player based on input from a dual axis (RightStick)</summary>
    /// <param name="_Sender">Where the event is coming from</param>
    /// <param name="_Args">The event arguments</param>
    private void Aim(XInput.Controller _Contorller, AxisArgs<Vector2> _Args)
    {
        //Don't bother with dead values (or else it will always try to turn back to 0 degrees)
        if (_Args.CurrState.magnitude <= 0 || !transform.parent.gameObject.activeSelf)
            return;

        //Calculate the rotation (in radians)
        float heading = Mathf.Atan2(-_Args.CurrState.y, _Args.CurrState.x);

        //Make the target rotation (quaternion)
        Quaternion targetRotation = Quaternion.Euler(0, heading * Mathf.Rad2Deg + 90, 0);

        //Smoothly rotate using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_settings.AimSpeed * Time.deltaTime);
    }
}
