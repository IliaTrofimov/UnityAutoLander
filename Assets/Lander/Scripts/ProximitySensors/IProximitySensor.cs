namespace ProximitySensors
{
    public interface IProximitySensor
	{
		public float GetDistance();
		public float Distance { get; }
		public string Uid { get; }
	}
}