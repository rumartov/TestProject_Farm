using UnityEditor;
using UnityEditorInternal;
using Random = UnityEngine.Random;

namespace CodeBase.Services.Randomizer
{
  public class RandomService : IRandomService
  {
    public int Next(int min, int max)
    {
      return Random.Range(min, max);
    }
  }
}