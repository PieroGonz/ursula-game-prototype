using Rive;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.VersionControl;
#endif
using UnityEngine;

public class RiveAnimationLister : MonoBehaviour
{
    private RiveTexture riveComponent;
    private StateMachine _riveStateMachine;
    private Rive.File m_file;
    private Artboard m_artboard;
    void Start()
    {

        riveComponent = transform.GetComponent<RiveTexture>();

        m_file = Rive.File.Load(riveComponent.asset);
        m_artboard = m_file.Artboard(0);
        _riveStateMachine = m_artboard?.StateMachine();

        SMIBool someTrigger = _riveStateMachine.GetBool("Dance Loop");

        Debug.Log(someTrigger.IsBoolean);

        someTrigger.Value = true;
    }
}
