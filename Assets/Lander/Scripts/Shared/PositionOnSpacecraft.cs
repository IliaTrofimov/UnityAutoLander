using System;

namespace Shared
{

    /// <summary>Положение детали на космическом корабле.</summary>
    /// <remarks>Данный тип можно использовать как ключ в словаре: хеш равен объединению хешей полей объекта.</remarks>
    // TODO: сделать синглтон
    [Serializable]
    public struct PositionOnSpacecraft
    {
        [UnityEngine.SerializeField] private AxisInfo.Axis _axis;
        [UnityEngine.SerializeField] private AxisInfo.Direction _direction;
        [UnityEngine.SerializeField] private Placement _placement;

        public AxisInfo.Axis Axis { get => _axis; private set => _axis = value; }

        public AxisInfo.Direction Direction { get => _direction; set => _direction = value; }

        public Placement Placement { get => _placement; set => _placement = value; }


        public PositionOnSpacecraft(AxisInfo.Axis axis, AxisInfo.Direction direction, Placement placement)
        {
            _axis = axis;
            _direction = direction;
            _placement = placement;
        }

        public override int GetHashCode() => HashCode.Combine(Axis, Direction, Placement);
        public override bool Equals(object obj) => obj is PositionOnSpacecraft p && p.Axis == Axis && p.Direction == Direction && p.Placement == Placement;
        public override string ToString() => $"{(Direction == AxisInfo.Direction.Negative ? '-' : '+')}{Axis}{Placement}";



        public static readonly PositionOnSpacecraft XPositiveTop = new PositionOnSpacecraft(AxisInfo.Axis.X, AxisInfo.Direction.Positive, Placement.Top);
        public static readonly PositionOnSpacecraft XPositiveBot = new PositionOnSpacecraft(AxisInfo.Axis.X, AxisInfo.Direction.Positive, Placement.Bottom);
        public static readonly PositionOnSpacecraft XNegativeTop = new PositionOnSpacecraft(AxisInfo.Axis.X, AxisInfo.Direction.Negative, Placement.Top);
        public static readonly PositionOnSpacecraft XNegativeBot = new PositionOnSpacecraft(AxisInfo.Axis.X, AxisInfo.Direction.Negative, Placement.Bottom);

        public static readonly PositionOnSpacecraft ZPositiveTop = new PositionOnSpacecraft(AxisInfo.Axis.Z, AxisInfo.Direction.Positive, Placement.Top);
        public static readonly PositionOnSpacecraft ZPositiveBot = new PositionOnSpacecraft(AxisInfo.Axis.Z, AxisInfo.Direction.Positive, Placement.Bottom);
        public static readonly PositionOnSpacecraft ZNegativeTop = new PositionOnSpacecraft(AxisInfo.Axis.Z, AxisInfo.Direction.Negative, Placement.Top);
        public static readonly PositionOnSpacecraft ZNegativeBot = new PositionOnSpacecraft(AxisInfo.Axis.Z, AxisInfo.Direction.Negative, Placement.Bottom);

        public static readonly PositionOnSpacecraft YPositiveTop = new PositionOnSpacecraft(AxisInfo.Axis.Y, AxisInfo.Direction.Positive, Placement.Top);
        public static readonly PositionOnSpacecraft YPositiveBot = new PositionOnSpacecraft(AxisInfo.Axis.Y, AxisInfo.Direction.Positive, Placement.Bottom);
        public static readonly PositionOnSpacecraft YNegativeTop = new PositionOnSpacecraft(AxisInfo.Axis.Y, AxisInfo.Direction.Negative, Placement.Top);
        public static readonly PositionOnSpacecraft YNegativeBot = new PositionOnSpacecraft(AxisInfo.Axis.Y, AxisInfo.Direction.Negative, Placement.Bottom);

    } 
}