//****************** 代码文件申明 ************************
//* 文件：IPlayerBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 01:25:48 星期一
//* 功能：nothing
//*****************************************************

using KooFrame;

namespace GameBuild
{
    public interface IPlayerBase : IArchitecture
    {
        public MovementBase movement { get; }

        //public AttackActionBase attackAction { get; }
        //public DeadActionBase deadAction { get; }
        //public ShootActionBase shootAction { get; }

        public RequestHandleBase requestHandle { get; }
        public RequestReceiverBase receiver { get; }
        public InvokerBase invoker { get; }
        public PlayerInputBase input { get; }
    }
}