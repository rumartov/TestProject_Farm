using CodeBase.Data;
using CodeBase.Services;

namespace Services.PersistentProgress
{
  public interface IPersistentProgressService : IService
  {
    PlayerProgress Progress { get; set; }
  }
}