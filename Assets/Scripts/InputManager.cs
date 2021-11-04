using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float HorizontalAxis
    {
        get
        {
            float axis = 0;
            if (Bindings[InputAction.LEFT].Pressed) axis--;
            if (Bindings[InputAction.RIGHT].Pressed) axis++;
            return axis;
        }
    }
    public float VerticalAxis
    {
        get
        {
            float axis = 0;
            if (Bindings[InputAction.DOWN].Pressed) axis--;
            if (Bindings[InputAction.UP].Pressed) axis++;
            return axis;
        }
    }
    public bool Jump { get => Bindings[InputAction.JUMP].Pressed; }
    public bool Dash { get => Bindings[InputAction.DASH].Pressed; }

    public Dictionary<InputAction, InputBind> Bindings { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        //Initialize bindings with all possible input uses
        Bindings = new Dictionary<InputAction, InputBind>();
        foreach (InputAction action in System.Enum.GetValues(typeof(InputAction)))
        {
            Bindings.Add(action, new InputBind(KeyCode.None));
        }
        AssignInputBindings();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (InputBind binding in Bindings.Values)
        {
            binding.Update();
        }

    //Testing
    /*if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      GameObject player = GameObject.Find("Player");
      TileTransformExplosion.MakeExplosion(player.transform.position, new Vector2(3, 3), new SpriteChangeTileTransformer());
    }
    if (Input.GetKeyDown(KeyCode.Mouse1))
    {
      GameObject player = GameObject.Find("Player");
      TileTransformExplosion.MakeExplosion(player.transform.position, new Vector2(3, 3), new NeutralTileTransformer());
    }*/
  }

    void FixedUpdate()
    {
        foreach (InputBind binding in Bindings.Values)
        {
            binding.FixedUpdate();
        }
    }

    private void AssignInputBindings()
    {
        Bindings[InputAction.LEFT].Key = KeyCode.A;
        Bindings[InputAction.RIGHT].Key = KeyCode.D;
        Bindings[InputAction.UP].Key = KeyCode.W;
        Bindings[InputAction.DOWN].Key = KeyCode.S;
        Bindings[InputAction.JUMP].Key = KeyCode.K;
        Bindings[InputAction.DASH].Key = KeyCode.L;
    }
}

public class InputBind
{
    public KeyCode Key { get; set; }
    public bool Pressed
    {
        get => pressed;
        private set
        {
            pressed = value;
            if (value)
                FramesPressed = 0;
        }
    }
    private bool pressed;
    public int FramesPressed { get; private set; }

    public InputBind(KeyCode keyCode)
    {
        Key = keyCode;
    }

    public void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            Pressed = true;
        }
        if (Input.GetKeyUp(Key))
        {
            Pressed = false;
        }
    }

    public void FixedUpdate()
    {
        if (Pressed)
            FramesPressed++;
    }
}

public enum InputAction
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    JUMP,
    DASH,
}
