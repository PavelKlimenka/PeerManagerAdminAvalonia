namespace Client.Models.Mappers
{
    public static class DepartmentMapper
    {
        public static DepartmentShort ToShortModel(this DepartmentModel model)
        {
            return new DepartmentShort()
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
