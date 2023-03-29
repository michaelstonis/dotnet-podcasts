namespace Microsoft.NetConf2021.Maui.Services;

public class ListenLaterService
{
    List<(Episode Episode, Show Show)> episodes;

    public ListenLaterService()
    {
        episodes = new List<(Episode Episode, Show Show)>();
    }

    public List<(Episode Episode, Show Show)> GetEpisodes()
    {
        return episodes;
    }

    public void Add(Episode episode, Show Show)
    {
        if (episodes.Any(ep => ep.Item1.Id == episode.Id))
            
            return;
        
        episodes.Add(new (episode, Show));
    }

    public void Remove(Episode episode)
    {
        var episodeToRemove = episodes.FirstOrDefault(ep => ep.Item1.Id == episode.Id);
        if (!episodeToRemove.Equals(default))
        {
            episodes.Remove(episodeToRemove);
        }
    }

    public bool IsInListenLater(Episode episode)
    {
        return episodes.Any(ep => ep.Item1.Id == episode.Id);
    }
    
    public bool IsInListenLater(Guid id)
    {
        return episodes.Any(ep => ep.Item1.Id == id);
    }
}
