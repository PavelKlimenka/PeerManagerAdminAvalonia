namespace Client.Models.Mappers
{
    public static class UserProfileMapper
    {
        public static UserProfileUpdateModel ToUpdateModel(this UserProfileModel model)
        {
            return new UserProfileUpdateModel()
            {
                Id = model.Id,
                ProjectName = model.ProjectName,
                OfficeLocationId = model.OfficeLocationId,
                Position = model.Position,
                Birthday = model.Birthday,
                Email = model.Email,
                Phone = model.Phone,
                FullName = model.FullName,
                EnglishLevel = model.EnglishLevel,
                EmploymentDate = model.EmploymentDate,
                DepartmentId = model.DepartmentId,
                ImageUrl = model.ImageUrl,
                RoomNumber = model.RoomNumber
            };
        }
    }
}
