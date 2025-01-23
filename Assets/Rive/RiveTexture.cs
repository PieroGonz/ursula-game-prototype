using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using Rive;

using LoadAction = UnityEngine.Rendering.RenderBufferLoadAction;
using StoreAction = UnityEngine.Rendering.RenderBufferStoreAction;
using System;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class RiveTexture : MonoBehaviour
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

        UnityEngine.Renderer cubeRenderer = GetComponent<UnityEngine.Renderer>();
        Material mat = cubeRenderer.material;
        mat.mainTexture = m_renderTexture;

        if (!FlipY())
        {
            // Flip the render texture vertically for OpenGL
            mat.mainTextureScale = new Vector2(1, -1);
            mat.mainTextureOffset = new Vector2(0, 1);
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
                Camera.main.AddCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
            }

        }

        InitializeStateMachineInputs();

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

        //SMIBool someTrigger = m_stateMachine.GetBool("Dance Loop");

        //Debug.Log(someTrigger.IsBoolean);

        //someTrigger.Value = true;

        HitTesting();

        if (m_stateMachine != null)
        {
            m_stateMachine.Advance(Time.deltaTime);
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

        if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return;

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
            m_wasMouseDown = false; Vector2 local = m_artboard.LocalCoordinate(mouseRiveScreenPos, new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), fit, alignment);
            m_stateMachine?.PointerUp(local);
        }
    }
    public void PlayAnimation(string animationName)
    {
        // Find the trigger by name and fire it
        foreach (var trigger in triggers)
        {
            if (trigger.name == animationName)
            {
                trigger.reference.Fire();
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
}
