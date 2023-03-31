namespace ProximitySensors
{
    public class ProximitySensorEvent
	{
		public string Uid { get; set; }
		public float Distance { get; set; }

		public ProximitySensorEvent(string uid, float dist)
		{
			Uid = uid;
			Distance = dist;
		}
	}
}