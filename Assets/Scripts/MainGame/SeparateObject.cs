public class SeparateObject : HitObject
{
    protected override void Awake()
    {
        
    }

    public override bool IsOver()
    {
        return Top < -Evaluation.Bad.Tolerance;
    }

    public override void OnFocus(KeyState state)
    {
    }
}
