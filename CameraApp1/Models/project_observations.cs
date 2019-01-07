using System.Collections.ObjectModel;

public class ProjectObservations
{
    private ObservableCollection<Observation> _projectobservations = new ObservableCollection<Observation>();

    public ObservableCollection<Observation> projectObservations
    {
        get => _projectobservations; 
        
    }

    public void AddObservation(Observation item)
    {
        _projectobservations.Add(item);
    }

    public void RemoveObservation(Observation item)
    {
        _projectobservations.Remove(item);
    }
}