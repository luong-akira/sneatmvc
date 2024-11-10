using System.Collections.Generic;

namespace Sneat.MVC.Models.DTO.Address
{
    public class ProvinceModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<DistrictModel> Districts { get; set; }
    }

    public class DistrictModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class WardModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}