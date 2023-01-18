[System.Serializable]
public class Net_OnSliderValueChange : NetMsg
{
    public Net_OnSliderValueChange()
    {
        OP = NetOP.OnSliderValueChange;
    }

    public byte Success { set; get; }
    public string Information { set; get; }

    public int ConnectionId { set; get; }
    public string Username { set; get; }
    public string Discriminator { set; get; }
    public string Token { set; get; }
}
