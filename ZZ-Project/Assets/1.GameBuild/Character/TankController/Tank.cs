using System;

namespace GameBuild
{
    public class Tank : CharacterBase<Tank>
    {
        protected override void RegisterIOC() { }

        protected override Type inputClassType { get; }
        protected override object[] inputArgs { get; }
        protected override Type movementClass => typeof(TankMove);
        protected override object[] movementArgs { get; }
        
        
    }
}