[System.Serializable]
public class Net_SliderValue : NetMsg
{
    public Net_SliderValue()
    {
        OP = NetOP.SliderValue;
    }

    public float MinRange { set; get; }
    public float MaxRange { set; get; }
    public float Feather { set; get; }

}
