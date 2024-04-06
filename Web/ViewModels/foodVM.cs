namespace Web.ViewModels
{
	public class foodVM
	{
        public foodVM(int _id , decimal _Price ,string _Title ,string _Description,string _Image,bool _IsAvailable)
        {
            id = _id ;
            Price=_Price ;
            Title=_Title ;
            Description=_Description ;
            Image = _Image ;
            IsAvailable = _IsAvailable ;
        
        }
        public int id { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsAvailable { get; set; }

    }
}
