using CodeBase.Data;
using Services.PersistentProgress;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
  public class PersistentProgressService : IPersistentProgressService
  {
    public PlayerProgress Progress { get; set; }
  }
}