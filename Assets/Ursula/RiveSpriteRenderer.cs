using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using Rive;

using LoadAction = UnityEngine.Rendering.RenderBufferLoadAction;
using StoreAction = UnityEngine.Rendering.RenderBufferStoreAction;
using System;

public class RiveSpriteRenderer : MonoBehaviour
{
    public Rive.Asset asset;
    public Fit fit = Fit.contain;
    public Alignment alignment = Alignment.Center;

    private RenderTexture m_renderTexture;
    private Rive.RenderQueue m_renderQueue;
    private Rive.Renderer m_riveRenderer;
    private CommandBuffer m_commandBuffer;

    private Rive.File m_file;
    private Artboard m_artboard;
    private StateMachine m_stateMachine;

    private Camera m_camera;
    private SpriteRenderer spriteRenderer;

    [Serializable]
    public struct SMITriggerDescriptor
    {
        public string name;
        public bool trigger;
        public SMITrigger reference;

        public SMITriggerDescriptor(string name, SMITrigger reference)
        {
            this.name = name;
            this.reference = reference;
            trigger = false;
        }
    }

    [Serializable]
    public struct SMIBoolDescriptor
    {
        public string name;
        public bool value;
        public SMIBool reference;

        public SMIBoolDescriptor(string name, bool value, SMIBool reference)
        {
            this.name = name;
            this.value = value;
            this.reference = reference;
        }
    }

    [Serializable]
    public class SMINumberDescriptor
    {
        public string name;
        public float value;
        public SMINumber reference;

        public SMINumberDescriptor(string name, float value, SMINumber reference)
        {
            this.name = name;
            this.value = value;
            this.reference = reference;
        }
    }

    [SerializeField]
    public List<SMITriggerDescriptor> triggers;
    [SerializeField]
    public List<SMIBoolDescriptor> booleans;
    [SerializeField]
    public List<SMINumberDescriptor> numbers;

    private static bool FlipY()
    {
        switch (UnityEngine.SystemInfo.graphicsDeviceType)
        {
            case UnityEngine.Rendering.GraphicsDeviceType.Metal:
            case UnityEngine.Rendering.GraphicsDeviceType.Direct3D11:
                return true;
            default:
                return false;
        }
    }

    private void Start()
    {
        m_renderTexture = new RenderTexture(TextureHelper.Descriptor(256, 256));
        m_renderTexture.Create();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            UpdateSpriteRenderer();
        }
        else
        {
            UnityEngine.Renderer meshRenderer = GetComponent<UnityEngine.Renderer>();
            if (meshRenderer != null)
            {
                Material mat = meshRenderer.material;
                mat.mainTexture = m_renderTexture;

                if (!FlipY())
                {
                    mat.mainTextureScale = new Vector2(1, -1);
                    mat.mainTextureOffset = new Vector2(0, 1);
                }
            }
        }

        m_renderQueue = new Rive.RenderQueue(m_renderTexture);
        m_riveRenderer = m_renderQueue.Renderer();

        if (asset != null)
        {
            m_file = Rive.File.Load(asset);
            m_artboard = m_file.Artboard(0);
            m_stateMachine = m_artboard?.StateMachine();
        }

        if (m_artboard != null && m_renderTexture != null)
        {
            m_riveRenderer.Align(fit, alignment, m_artboard);
            m_riveRenderer.Draw(m_artboard);

            m_commandBuffer = m_riveRenderer.ToCommandBuffer();
            m_commandBuffer.SetRenderTarget(m_renderTexture);
            m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
            m_riveRenderer.AddToCommandBuffer(m_commandBuffer);

            m_camera = Camera.main;
            if (m_camera != null)
            {
                m_camera.AddCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
            }
        }

        InitializeStateMachineInputs();
        PrintStateMachineInputs();
    }

    private void PrintStateMachineInputs()
    {
        string booleansEnum = "Booleans: ";
        foreach (var boolean in booleans)
        {
            booleansEnum += boolean.name + ", ";
        }

        string triggersEnum = "Triggers: ";
        foreach (var trigger in triggers)
        {
            triggersEnum += trigger.name + ", ";
        }

        // Print in a single console line
        Debug.Log(booleansEnum.TrimEnd(',', ' ') + " | " + triggersEnum.TrimEnd(',', ' '));
    }

    private void UpdateSpriteRenderer()
    {
        if (spriteRenderer != null && m_renderTexture != null)
        {
            RenderTexture.active = m_renderTexture;

            Texture2D texture2D = new Texture2D(m_renderTexture.width, m_renderTexture.height, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), 0, 0);
            texture2D.Apply();

            spriteRenderer.sprite = Sprite.Create(
                texture2D,
                new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f)
            );

            RenderTexture.active = null;
        }
    }

    private void InitializeStateMachineInputs()
    {
        booleans = new List<SMIBoolDescriptor>();
        triggers = new List<SMITriggerDescriptor>();
        numbers = new List<SMINumberDescriptor>();

        var inputs = m_stateMachine.Inputs();
        foreach (var input in inputs)
        {
            switch (input)
            {
                case SMITrigger smiTrigger:
                    {
                        var descriptor = new SMITriggerDescriptor(smiTrigger.Name, smiTrigger);
                        triggers.Add(descriptor);
                        break;
                    }
                case SMIBool smiBool:
                    {
                        var descriptor = new SMIBoolDescriptor(smiBool.Name, smiBool.Value, smiBool);
                        booleans.Add(descriptor);
                        break;
                    }
                case SMINumber smiNumber:
                    {
                        var descriptor = new SMINumberDescriptor(smiNumber.Name, smiNumber.Value, smiNumber);
                        numbers.Add(descriptor);
                        break;
                    }
            }
        }
    }

    private void Update()
    {
        //HitTesting();

        if (m_stateMachine != null)
        {
            m_stateMachine.Advance(Time.deltaTime);

        
        }

        if (spriteRenderer != null && m_renderTexture != null)
        {
            UpdateSpriteRenderer();
        }
    }

    private void OnValidate()
    {
        // State machine triggers
        var triggerDidChange = false;
        foreach (var inspectorInput in triggers)
        {
            if (inspectorInput.reference == null) continue;
            if (inspectorInput.trigger == true)
            {
                inspectorInput.reference.Fire();
                triggerDidChange = true;
            }
        }

        if (triggerDidChange)
        {
            var updatedTriggers = new List<SMITriggerDescriptor>();
            foreach (var inspectorInput in triggers)
            {
                updatedTriggers.Add(new SMITriggerDescriptor(inspectorInput.name, inspectorInput.reference));
            }
            triggers = updatedTriggers;
        }

        // State machine booleans
        foreach (var inspectorInput in booleans)
        {
            if (inspectorInput.reference == null) continue;
            if (inspectorInput.value == inspectorInput.reference.Value) continue;
            inspectorInput.reference.Value = inspectorInput.value;
        }

        // State machine numbers
        foreach (var inspectorInput in numbers)
        {
            if (inspectorInput.reference == null) continue;
            if (inspectorInput.value == inspectorInput.reference.Value) continue;
            inspectorInput.reference.Value = inspectorInput.value;
        }
    }

    bool m_wasMouseDown = false;
    private Vector2 m_lastMousePosition;

    void HitTesting()
    {
        Camera camera = Camera.main;

        if (camera == null || m_renderTexture == null || m_artboard == null) return;

        if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) return;

        UnityEngine.Renderer rend = hit.transform.GetComponent<UnityEngine.Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Vector2 pixelUV = hit.textureCoord;

        pixelUV.x *= m_renderTexture.width;
        pixelUV.y *= m_renderTexture.height;

        Vector3 mousePos = camera.ScreenToViewportPoint(Input.mousePosition);
        Vector2 mouseRiveScreenPos = new(mousePos.x * camera.pixelWidth, (1 - mousePos.y) * camera.pixelHeight);

        if (m_lastMousePosition != mouseRiveScreenPos || transform.hasChanged)
        {
            Vector2 local = m_artboard.LocalCoordinate(pixelUV, new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), fit, alignment);
            m_stateMachine?.PointerMove(local);
            m_lastMousePosition = mouseRiveScreenPos;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 local = m_artboard.LocalCoordinate(pixelUV, new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), fit, alignment);
            m_stateMachine?.PointerDown(local);
            m_wasMouseDown = true;
        }
        else if (m_wasMouseDown)
        {
            m_wasMouseDown = false;
            Vector2 local = m_artboard.LocalCoordinate(mouseRiveScreenPos, new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), fit, alignment);
            m_stateMachine?.PointerUp(local);
        }
    }

    private string lastTriggerName;
    private bool isAnimationPlaying = false;

    public void PlayAnimation(string animationName)
    {
        // If an animation is currently playing, ignore new inputs

        PlayAnimationImmediately(animationName);

        /*(if (isAnimationPlaying)
        {
            return; // Do nothing if an animation is currently playing
        }

        // If the animation is the same as the last one, play it immediately
        if (lastTriggerName == animationName)
        {
            PlayAnimationImmediately(animationName);
            return;
        }

        // Otherwise, queue the new trigger to be fired after the delay
        isAnimationPlaying = true; // Block further inputs
        lastTriggerName = animationName;
        StartCoroutine(ActivateTriggerAfterDelay(animationName, 0.3f));*/
    }

    private void PlayAnimationImmediately(string animationName)
    {
        // First, deactivate all boolean animations that are not the one we want to play
        foreach (var boolean in booleans)
        {
            boolean.reference.Value = false;
        }

        // Activate the desired boolean animation, if it matches the name
        foreach (var boolean in booleans)
        {
            if (boolean.name == animationName)
            {
                boolean.reference.Value = true;
                return; // Exit the method after activating the boolean to avoid conflicts
            }
        }

        // If no boolean was activated, we assume it's a trigger animation
        foreach (var trigger in triggers)
        {
            if (trigger.name == animationName)
            {
                trigger.reference.Fire();
                break;
            }
        }
    }

    private IEnumerator ActivateTriggerAfterDelay(string animationName, float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // Play the animation immediately after the delay
        PlayAnimationImmediately(animationName);

        // Wait for the animation to "finish" (adjust duration to match your animation)
        yield return new WaitForSeconds(0.5f); // Replace with actual animation duration

        // Allow new inputs after the animation completes
        isAnimationPlaying = false;
    }

    public void StopAnimation(string animationName)
    {
        foreach (var trigger in triggers)
        {
            if (trigger.name == animationName)
            {
                trigger.reference.Fire();
                break;
            }
        }

        foreach (var boolean in booleans)
        {
            if (boolean.name == animationName)
            {
                boolean.reference.Value = false;
                break;
            }
        }
    }

    private void OnDisable()
    {
        if (m_camera != null && m_commandBuffer != null)
        {
            m_camera.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
        }
    }

    void OnDestroy()
    {
        // Release the RenderTexture when it's no longer needed
        if (m_renderTexture != null)
            m_renderTexture.Release();
    }

    public enum BooleanActions
    {
        HatEquipped,
        CheeksVisibility,
        ArmRightEquipped,
        ArmLeftEquipped,
        EyeFollowUser,
        EyeFollowTarget,
        BodyFollowTarget,
        DebugVisualizer,
        TeethVisible,
        ListeningLoop,
        JumpingJacksLoop,
        ScaredLoop,
        EyesClosedLoop,
        DeepBreathingLoop,
        BodyFocusLoop,
        SleepLoop,
        LoveLoop,
        PledgeLoop,
        ExcitedLoop,
        IsInteracting,
        ExcitedLoop2,
        IsIdle,
        DanceLoop,
        DanceLoop2,
        LookingDownLoop,
        ThinkingLoop,
        SideToSideLoop,
        TalkingLoop,
        PointerDown,
        TalkingBouncyLoop,
        FlyingRightLoop,
        PettingLoop
    }


    public enum TriggerActions
    {
        BodyRoundClose,
        BodyRollClose,
        BodyListeningNext,
        BodyDrag,
        BodyBounce,
        StartTap,
        NextTap,
        BodyStaticPose,
        BodyListeningStart,
        EyeBlink,
        EyeExpressionDefault,
        EyeAngry,
        EyeClosedHappy,
        EyeSad,
        ArmRightWave,
        EyeSurprised,
        EyeThinking,
        EyeLove,
        EyeClosedCalm,
        BodyRoundBounce,
        EyeClosedSqueeze,
        EyeCute,
        HeadBounce,
        HeadNodYes,
        HatSelectWizard,
        BodyRoundOpen,
        BodyRollOpen,
        BodyStretch,
        BodyJump,
        HeadNodNo,
        ArmLeftWave,
        RootMove,
        RootMoveSlow,
        RootMoveFast,
        EyeSpiral,
        ColorBlue,
        ColorDarkBlue,
        ColorPurple,
        ExitBodyAnimation,
        ExitBodyAnimationBounce,
        BodyIdleLoop,
        BodyIdle2Loop,
        BodyIdle3Loop,
        BodyIdleBouncyLoop,
        BodyIdleWiggleLoop,
        BodyIdleWiggle2Loop,
        BodySwayingLoop,
        BodyDanceLoop,
        BodyDance2Loop,
        BodyFlyingUp,
        BodyDanceBounceLoop,
        ColorPink,
        BodyFlyingDown,
        BodyFlyingRightUp,
        EyeSquint,
        BodyFlyingRight,
        BodyJumpLoop,
        ColorRed,
        BodyFlyingRightDown,
        BodyExcitedJumpLoop,
        BodyFlyingLeftUp,
        BodyFlyingLeft,
        BodyFlyingLeftDown,
        ColorOrange,
        BodyExcitedLoop,
        BodyLookDownLoop,
        ColorYellow,
        ColorGreen,
        BodyHeadScratchLoop,
        BodyHeadTiltLoop,
        BodyHandsBackLoop,
        BodySideScratchLoop,
        AgeOff,
        Age12,
        Age11,
        Age10,
        Age9,
        Age8,
        Age7,
        Age6,
        CancelOneShotAnimation,
        RiseUp,
        Shake,
        FootShake,
        NoddingYes,
        LevelUp,
        LevelUpAlt,
        Wave,
        LookAround,
        Happy,
        Curious,
        FaceNone,
        FaceHappy,
        FaceNeutral,
        FaceSad,
        Tap,
        Wiggle
    }


    private static readonly Dictionary<BooleanActions, string> booleanActionStrings = new Dictionary<BooleanActions, string>
{
    { BooleanActions.HatEquipped, "Hat Equipped" },
    { BooleanActions.CheeksVisibility, "Cheeks Visibility" },
    { BooleanActions.ArmRightEquipped, "Arm Right Equipped" },
    { BooleanActions.ArmLeftEquipped, "Arm Left Equipped" },
    { BooleanActions.EyeFollowUser, "Eye Follow User" },
    { BooleanActions.EyeFollowTarget, "Eye Follow Target" },
    { BooleanActions.BodyFollowTarget, "Body Follow Target" },
    { BooleanActions.DebugVisualizer, "Debug Visualizer" },
    { BooleanActions.TeethVisible, "Teeth Visible" },
    { BooleanActions.ListeningLoop, "Listening Loop" },
    { BooleanActions.JumpingJacksLoop, "Jumping Jacks Loop" },
    { BooleanActions.ScaredLoop, "Scared Loop" },
    { BooleanActions.EyesClosedLoop, "Eyes Closed Loop" },
    { BooleanActions.DeepBreathingLoop, "Deep Breathing Loop" },
    { BooleanActions.BodyFocusLoop, "Body Focus Loop" },
    { BooleanActions.SleepLoop, "Sleep Loop" },
    { BooleanActions.LoveLoop, "Love Loop" },
    { BooleanActions.PledgeLoop, "Pledge Loop" },
    { BooleanActions.ExcitedLoop, "Excited Loop" },
    { BooleanActions.IsInteracting, "Is Interacting" },
    { BooleanActions.ExcitedLoop2, "Excited Loop 2" },
    { BooleanActions.IsIdle, "Is Idle" },
    { BooleanActions.DanceLoop, "Dance Loop" },
    { BooleanActions.DanceLoop2, "Dance Loop 2" },
    { BooleanActions.LookingDownLoop, "Looking Down Loop" },
    { BooleanActions.ThinkingLoop, "Thinking Loop" },
    { BooleanActions.SideToSideLoop, "Side To Side Loop" },
    { BooleanActions.TalkingLoop, "Talking Loop" },
    { BooleanActions.PointerDown, "Pointer Down" },
    { BooleanActions.TalkingBouncyLoop, "Talking Bouncy Loop" },
    { BooleanActions.FlyingRightLoop, "Flying Right Loop" },
    { BooleanActions.PettingLoop, "Petting Loop" }
};


    private static readonly Dictionary<TriggerActions, string> triggerActionStrings = new Dictionary<TriggerActions, string>
{
    { TriggerActions.BodyRoundClose, "Body Round Close" },
    { TriggerActions.BodyRollClose, "Body Roll Close" },
    { TriggerActions.BodyListeningNext, "Body Listening Next" },
    { TriggerActions.BodyDrag, "Body Drag" },
    { TriggerActions.BodyBounce, "Body Bounce" },
    { TriggerActions.StartTap, "Start Tap" },
    { TriggerActions.NextTap, "Next Tap" },
    { TriggerActions.BodyStaticPose, "Body Static Pose" },
    { TriggerActions.BodyListeningStart, "Body Listening Start" },
    { TriggerActions.EyeBlink, "Eye Blink" },
    { TriggerActions.EyeExpressionDefault, "Eye Expression Default" },
    { TriggerActions.EyeAngry, "Eye Angry" },
    { TriggerActions.EyeClosedHappy, "Eye Closed Happy" },
    { TriggerActions.EyeSad, "Eye Sad" },
    { TriggerActions.ArmRightWave, "Arm Right Wave" },
    { TriggerActions.EyeSurprised, "Eye Surprised" },
    { TriggerActions.EyeThinking, "Eye Thinking" },
    { TriggerActions.EyeLove, "Eye Love" },
    { TriggerActions.EyeClosedCalm, "Eye Closed Calm" },
    { TriggerActions.BodyRoundBounce, "Body Round Bounce" },
    { TriggerActions.EyeClosedSqueeze, "Eye Closed Squeeze" },
    { TriggerActions.EyeCute, "Eye Cute" },
    { TriggerActions.HeadBounce, "Head Bounce" },
    { TriggerActions.HeadNodYes, "Head Nod Yes" },
    { TriggerActions.HatSelectWizard, "Hat Select Wizard" },
    { TriggerActions.BodyRoundOpen, "Body Round Open" },
    { TriggerActions.BodyRollOpen, "Body Roll Open" },
    { TriggerActions.BodyStretch, "Body Stretch" },
    { TriggerActions.BodyJump, "Body Jump" },
    { TriggerActions.HeadNodNo, "Head Nod No" },
    { TriggerActions.ArmLeftWave, "Arm Left Wave" },
    { TriggerActions.RootMove, "Root Move" },
    { TriggerActions.RootMoveSlow, "Root Move Slow" },
    { TriggerActions.RootMoveFast, "Root Move Fast" },
    { TriggerActions.EyeSpiral, "Eye Spiral" },
    { TriggerActions.ColorBlue, "Color Blue" },
    { TriggerActions.ColorDarkBlue, "Color Dark Blue" },
    { TriggerActions.ColorPurple, "Color Purple" },
    { TriggerActions.ExitBodyAnimation, "Exit Body Animation" },
    { TriggerActions.ExitBodyAnimationBounce, "Exit Body Animation Bounce" },
    { TriggerActions.BodyIdleLoop, "Body Idle Loop" },
    { TriggerActions.BodyIdle2Loop, "Body Idle 2 Loop" },
    { TriggerActions.BodyIdle3Loop, "Body Idle 3 Loop" },
    { TriggerActions.BodyIdleBouncyLoop, "Body Idle Bouncy Loop" },
    { TriggerActions.BodyIdleWiggleLoop, "Body Idle Wiggle Loop" },
    { TriggerActions.BodyIdleWiggle2Loop, "Body Idle Wiggle 2 Loop" },
    { TriggerActions.BodySwayingLoop, "Body Swaying Loop" },
    { TriggerActions.BodyDanceLoop, "Body Dance Loop" },
    { TriggerActions.BodyDance2Loop, "Body Dance 2 Loop" },
    { TriggerActions.BodyFlyingUp, "Body Flying Up" },
    { TriggerActions.BodyDanceBounceLoop, "Body Dance Bounce Loop" },
    { TriggerActions.ColorPink, "Color Pink" },
    { TriggerActions.BodyFlyingDown, "Body Flying Down" },
    { TriggerActions.BodyFlyingRightUp, "Body Flying Right Up" },
    { TriggerActions.EyeSquint, "Eye Squint" },
    { TriggerActions.BodyFlyingRight, "Body Flying Right" },
    { TriggerActions.BodyJumpLoop, "Body Jump Loop" },
    { TriggerActions.ColorRed, "Color Red" },
    { TriggerActions.BodyFlyingRightDown, "Body Flying Right Down" },
    { TriggerActions.BodyExcitedJumpLoop, "Body Excited Jump Loop" },
    { TriggerActions.BodyFlyingLeftUp, "Body Flying Left Up" },
    { TriggerActions.BodyFlyingLeft, "Body Flying Left" },
    { TriggerActions.BodyFlyingLeftDown, "Body Flying Left Down" },
    { TriggerActions.ColorOrange, "Color Orange" },
    { TriggerActions.BodyExcitedLoop, "Body Excited Loop" },
    { TriggerActions.BodyLookDownLoop, "Body Look Down Loop" },
    { TriggerActions.ColorYellow, "Color Yellow" },
    { TriggerActions.ColorGreen, "Color Green" },
    { TriggerActions.BodyHeadScratchLoop, "Body Head Scratch Loop" },
    { TriggerActions.BodyHeadTiltLoop, "Body Head Tilt Loop" },
    { TriggerActions.BodyHandsBackLoop, "Body Hands Back Loop" },
    { TriggerActions.BodySideScratchLoop, "Body Side Scratch Loop" }
};


    public static string GetBooleanActionString(BooleanActions action)
    {
        return booleanActionStrings[action];
    }

    public static string GetTriggerActionString(TriggerActions action)
    {
        return triggerActionStrings[action];
    }

}
