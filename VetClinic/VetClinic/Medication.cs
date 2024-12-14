using VetClinic;
using VetClinic.Exceptions;
using NullReferenceException = System.NullReferenceException;

public enum Form { Pill, Injection, Cream, Powder, Syrup }

public class Medication : StoredObject<Medication>, IIdentifiable
{
    public int Id { get; set; }

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyStringException("Name is mandatory and cannot be empty.");
            _name = value;
        }
    }
    public Form Form { get; set; }

    private List<Dose> _doses = new();
        
    public List<Dose> GetDoses()
    {
        return new List<Dose>(_doses);
    }

    private List<Medication> _components = new();
    
    private List<Medication> _componentOf = new();
    
    public List<Medication> GetComponents()
    {
        return new List<Medication>(_components);
    }
    
    public List<Medication> GetComponentOf()
    {
        return new List<Medication>(_componentOf);
    }
    
    /// <summary>
    /// Adds the specified Medication as a component of this Medication, maintaining a reflexive (self-referential) association.
    /// Throws exceptions if a duplicate or cyclic relationship is detected, or if the component is this same Medication.
    /// </summary>
    public void AddComponent(Medication medication)
    {
        // Ensure that the specified medication is not already in _components.
        // If it is, throw a DuplicatesException to prevent adding duplicates.
        if (_components.Contains(medication))
            throw new DuplicatesException($"Medication '{medication.Name}' already exists as a component.");

        // Check if the medication we're trying to add is actually the current object (this).
        // A medication cannot be a component of itself, so throw an exception if they match.
        if (medication == this)
            throw new InvalidOperationException("A medication cannot be a component of itself.");

        // Prevent a simple cycle: if the specified medication's _components already contains this Medication,
        // it implies a direct bidirectional cycle (this → medication and medication → this).
        if (medication.GetComponents().Contains(this))
            throw new InvalidOperationException("Cyclic dependency detected.");

        // If all checks pass, add the medication to our _components list.
        _components.Add(medication);

        // Also add this medication to the other medication's _componentOf list,
        // ensuring the relationship is maintained bidirectionally.
        medication._componentOf.Add(this);
    }

    /// <summary>
    /// Removes the specified Medication from this Medication's list of components,
    /// ensuring the reflexive association is correctly updated on both sides.
    /// </summary>
    public void RemoveComponent(Medication medication)
    {
        // Check if the specified medication is actually in this Medication's _components list.
        // If not, we throw a NotFoundException because we can't remove something that isn't there.
        if (!_components.Contains(medication))
            throw new NotFoundException($"Medication '{medication.Name}' not found in the components list.");

        // Remove the medication from our _components list.
        _components.Remove(medication);

        // Also remove this medication from the other medication's _componentOf list,
        // ensuring the relationship is no longer tracked on either side.
        medication._componentOf.Remove(this);
    }

    /// <summary>
    /// Associates this Medication with a given Prescription by creating or retrieving a Dose.
    /// Ensures that this Medication is tracked by the Prescription and vice versa.
    /// </summary>
    public void AddPrescription(Prescription prescription, string doseDescription, double doseAmount)
    {
        // If this Medication already has a dose for the given Prescription, it means the association already exists.
        // We throw a DuplicatesException to avoid duplicates.
        if (_doses.Any(x => x.GetPrescription().Equals(prescription)))
            throw new DuplicatesException("Prescription already exists in the list.");

        // Attempt to find if a Dose already exists in the Prescription for this Medication.
        var dose = prescription.GetDoses().Find(x => x.GetMedication().Equals(this));

        // If no matching Dose is found, create a new Dose object to represent the association.
        if (dose == null)
            dose = new Dose(doseDescription, doseAmount, this, prescription);

        // If the Prescription does not know about this Dose, tell the Prescription to add it.
        // This ensures the Prescription also tracks the Dose, maintaining bidirectional consistency.
        if (!prescription.GetDoses().Contains(dose))
            prescription.AddMedication(this, doseDescription, doseAmount);
    }

    /// <summary>
    /// Overload that accepts an existing Dose object, adding it to this Medication's list
    /// and ensuring the Dose is also recognized by the associated Prescription.
    /// </summary>
    public void AddPrescription(Dose dose)
    {
        // If this Medication already contains a Dose for the same Prescription or the same Medication,
        // a DuplicatesException is thrown to avoid duplicates.
        if (_doses.Any(x => x.GetPrescription().Equals(dose.GetPrescription()) 
                            || x.GetMedication().Equals(dose.GetMedication())))
            throw new DuplicatesException("Prescription already exists in the list.");

        // Add the provided Dose to this Medication's internal list of Doses.
        _doses.Add(dose);

        // If the associated Prescription doesn't recognize this Dose yet, add it there as well
        // to maintain bidirectional consistency.
        if (!dose.GetPrescription().GetDoses().Contains(dose))
            dose.GetPrescription().AddMedication(dose);
    }

    /// <summary>
    /// Removes the association between this Medication and a given Prescription,
    /// ensuring the corresponding Dose object is also removed from both sides.
    /// </summary>
    public void RemovePrescription(Prescription prescription)
    {
        // Throw NullReferenceException if the provided Prescription is null, to prevent invalid usage.
        if (prescription == null)
            throw new NullReferenceException();

        // Attempt to find a Dose in this Medication's _doses list corresponding to the given Prescription.
        var dose = _doses.Find(x => x.GetPrescription().Equals(prescription));

        // If not found, throw NotFoundException indicating there's no matching Dose for that Prescription.
        if (dose == null)
            throw new NotFoundException("This prescription in not associated to the medication.");

        // Remove the Dose from this Medication's list.
        _doses.Remove(dose);

        // If the Prescription still tracks this Dose, remove the Medication from that Prescription as well
        // to maintain bidirectional consistency.
        if (prescription.GetDoses().Contains(dose))
            prescription.RemoveMedication(this);
    }

    /// <summary>
    /// Removes this Medication entirely from the system, including all Dose associations.
    /// Ensures each associated Dose is properly removed before deleting the Medication.
    /// </summary>
    public void RemoveMedication()
    {
        // Check if this Medication is actually in the global _extent list before removal.
        if (!_extent.Contains(this))
            throw new NotFoundException("Medication not found in the list.");

        // Copy the list of Doses to avoid modifying the collection while iterating.
        var doses = new List<Dose>(_doses);

        // For each Dose that references this Medication, call RemoveDose(),
        // which removes the Dose from the global extent and breaks the link to the Prescription.
        foreach (var dose in doses)
        {
            dose.RemoveDose();
        }

        // Finally, remove this Medication from the global extent, effectively removing it from the system.
        _extent.Remove(this);
    }
    
    public Medication() { }

    public Medication(string name, Form form)
    {
        Name = name;
        Form = form;
        AddToExtent(this);
        _extent.Add(this);
    }

    public override string ToString()
    {
        return $"Id={Id}, Name={Name}, Form={Form}";
    }
    
}
