namespace Telemetry
{
    public class SensorInfo
    {
        public string Uid { get; set; }
        public float Distance { get; set; }

        public override int GetHashCode()
        {
            return Uid.GetHashCode();
        }


        public override bool Equals(object obj)
        {
            return obj is SensorInfo i && i.Uid == Uid;
        }
    }

}