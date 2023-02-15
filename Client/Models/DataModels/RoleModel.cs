using System;

namespace Client.Models.DataModels;

public class RoleModel : IEquatable<RoleModel>
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }

    public bool Equals(RoleModel? other)
    {
        if(ReferenceEquals(null, other)) return false;

        return this.Id == other.Id;
    }
}