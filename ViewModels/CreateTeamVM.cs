namespace BizLandTemplate.ViewModels
{
    public class CreateTeamVM
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
        public string TwitterLink { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string LinkedinLink { get; set; }
        public int PositionId { get; set; }
    }
}
