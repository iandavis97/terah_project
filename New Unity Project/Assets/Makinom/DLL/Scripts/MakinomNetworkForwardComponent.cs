
using UnityEngine;
using UnityEngine.Networking;
using Makinom;
using Makinom.Networking;
using System.Collections.Generic;

namespace Makinom.Behaviours
{
	[AddComponentMenu("Makinom/Machines/Makinom Network Forward")]
	public class MakinomNetworkForwardComponent : NetworkBehaviour
	{
		public override void OnNetworkDestroy()
		{
			AutoMachineComponent[] machine = this.GetComponents<AutoMachineComponent>();
			for(int i=0; i<machine.Length; i++)
			{
				machine[i].OnNetworkDestroy();
			}
		}
		
		public override void OnStartAuthority()
		{
			AutoMachineComponent[] machine = this.GetComponents<AutoMachineComponent>();
			for(int i=0; i<machine.Length; i++)
			{
				machine[i].OnStartAuthority();
			}
		}
		
		public override void OnStartClient()
		{
			AutoMachineComponent[] machine = this.GetComponents<AutoMachineComponent>();
			for(int i=0; i<machine.Length; i++)
			{
				machine[i].OnStartClient();
			}
		}
		
		public override void OnStartLocalPlayer()
		{
			AutoMachineComponent[] machine = this.GetComponents<AutoMachineComponent>();
			for(int i=0; i<machine.Length; i++)
			{
				machine[i].OnStartLocalPlayer();
			}
		}
		
		public override void OnStartServer()
		{
			AutoMachineComponent[] machine = this.GetComponents<AutoMachineComponent>();
			for(int i=0; i<machine.Length; i++)
			{
				machine[i].OnStartServer();
			}
		}
	}
}
