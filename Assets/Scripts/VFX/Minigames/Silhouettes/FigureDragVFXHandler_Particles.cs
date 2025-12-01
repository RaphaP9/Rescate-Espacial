using UnityEngine;
using UnityEngine.VFX;

public class FigureDragVFXHandler_Particles : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private FigureHandler figureHandler;
    [SerializeField] private VisualEffect visualEffect;
    private void OnEnable()
    {
        figureHandler.OnThisFigureDragStart += FigureHandler_OnThisFigureDragStart;
        figureHandler.OnThisFigureDragEnd += FigureHandler_OnThisFigureDragEnd;
    }

    private void OnDisable()
    {
        figureHandler.OnThisFigureDragStart -= FigureHandler_OnThisFigureDragStart;
        figureHandler.OnThisFigureDragEnd -= FigureHandler_OnThisFigureDragEnd;
    }

    private void Start()
    {
        StopVFX();
    }

    private void PlayVFX() => visualEffect.Play();
    private void StopVFX() => visualEffect.Stop();  
    private void FigureHandler_OnThisFigureDragStart(object sender, System.EventArgs e)
    {
        PlayVFX();
    }
    private void FigureHandler_OnThisFigureDragEnd(object sender, System.EventArgs e)
    {
        StopVFX();
    }
}
