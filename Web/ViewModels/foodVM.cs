namespace Web.ViewModels
{
	public class foodVM
	{
        public foodVM(int _id , decimal _Price ,string _Title ,double _Rating,string _Image,bool _IsAvailable)
        {
            id = _id ;
            Price=_Price ;
            Title=_Title ;
            Rating = _Rating;
            Image = _Image ;
            IsAvailable = _IsAvailable ;
        
        }

        public foodVM()
        {
            
        }
        public int id { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public string Image { get; set; }
        public bool IsAvailable { get; set; }

    }
}
