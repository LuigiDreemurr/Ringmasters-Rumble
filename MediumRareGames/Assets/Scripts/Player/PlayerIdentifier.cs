using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlayerIdentifier : MonoBehaviour
{
    [SerializeField] private PlayerInfo m_info;
    [Space]
    [SerializeField]
    private Text m_text;
    [SerializeField] private Text m_timer;
    [SerializeField] private Text m_timerShadow;
    [SerializeField] private Image m_image;
    [SerializeField] private SkinnedMeshRenderer m_playerRenderer;
    [Space]
    [SerializeField]
    private bool m_fadeAtStart = false;
    [SerializeField] [HideInInspector] private float m_fadeDelay = 4.5f;
    [SerializeField] [HideInInspector] private float m_fadeDuration = 1.5f;

    Color m_timerShadowColor;

    private Color Transparent
    {
        get
        {
            Color trans = m_info.Color;
            trans.a = 0;
            return trans;
        }
    }

    public PlayerInfo Info
    {
        get { return m_info; }
        set { m_info = value; }
    }

    public void ShowText()
    {
        //Is just resetting the alpha
        m_text.color = m_info.Color;
        m_timer.color = m_info.Color;
        m_timerShadow.color = m_timerShadowColor;
    }

    public void HideText()
    {
        m_text.color = Transparent;
        m_timer.color = Transparent;
        m_timerShadow.color = Transparent;
    }

    public void FadeText(float _Delay, float _Duration)
    {
        StartCoroutine(FadeTextRoutine(_Delay, _Duration));
    }

    private IEnumerator FadeTextRoutine(float _Delay, float _Duration)
    {
        yield return new WaitForSeconds(_Delay);

        Color transparent = Transparent;

        float elapsedTime = 0f;

        while (elapsedTime < _Duration)
        {
            elapsedTime += Time./*unscaledD*/deltaTime;
            m_text.color = Color.Lerp(m_info.Color, transparent, elapsedTime / _Duration);
            m_timer.color = Color.Lerp(m_info.Color, transparent, elapsedTime / _Duration);
            m_timerShadow.color = Color.Lerp(m_timerShadowColor, transparent, elapsedTime / _Duration);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    /// <summary>Set the text/image color</summary>
    private void RefreshColor()
    {
        m_text.color = m_info.Color;
        m_timer.color = m_info.Color;
        m_timerShadow.color = m_timerShadowColor;
        m_image.color = m_info.Color;
        m_playerRenderer.material.color = m_info.Color;
    }

    /// <summary>Set the text</summary>
    private void RefreshNumber()
    {
        m_text.text = "P" + (((int)m_info.Index) + 1).ToString();
    }

    /// <summary>Initialization</summary>
    void Start()
    {
        RefreshColor();
        RefreshNumber();

        m_timerShadowColor = m_timerShadow.color;

        Instantiate(m_info.Character.Prefab, m_playerRenderer.transform);

        if (m_fadeAtStart)
            FadeText(m_fadeDelay, m_fadeDuration);
    }
}
