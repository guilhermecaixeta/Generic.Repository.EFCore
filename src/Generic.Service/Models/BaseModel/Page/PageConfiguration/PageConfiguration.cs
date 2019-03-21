namespace Generic.Service.Models.BaseModel.Page.PageConfiguration
{
    public struct PageConfiguration : IPageConfiguration
    {
        public int page { get ; set ; }
        public int size { get ; set ; }
        public string sort { get ; set ; }
        public string order { get ; set ; }
    }
}