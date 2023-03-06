using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public class MobileInputService : InputService
  {
    
    public override Vector2 Axis => SimpleInputAxis();
  }
}