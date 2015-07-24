# UrlShortener

This is a Url Shortener Web Application developed to satisfy the partial requirement for job Application process at Mindvalley Malaysia Sdn Bhd. Url Shortener is often available and provided by large software cooperations such as google's https://goo.gl/ and the popularly known <a href="https://bitly.com/">bitly<a/>

#Design Description

The project is strictly based on Model View Controller or polularly known as MVC Design . The Solution has 3 projects name UrlShortener.DOmain which is our main model class. UrlShortener.WebUI is solely responsible for our view and and controller classes . UrlShorter.Test project is where our UnitTest were performed .  

#Design flow 

Like mentioned above the project follows the popular MVC design . The important code folow i would like to emphasize here is the controller dependency injection and the Dependency Inversion Principle of the SOLID principle applied in the model class. I utilize ninject dependency injector nuget to inject our data as served from the model to each and every controller.

- <b>UrlShortener.Domain</b>
   
  Our model class. The use of Dependency Inversion principle . Below is our model class

Url.cs

<pre>

 public class Url
    {
        public int UrlId { get; set; }
        public string OriginalUrl { get; set; }
        public string UrlCode { get; set; }
        public DateTime PostedDate { get; set; }
        public string IpAddress { get; set; }
    }

</pre>

and inside our Abstract folder is our Interface class.

<pre>
public interface IUrlsRepository
    {
        IQueryable<Url> Urls { get; }

        bool AddUrl(Url url);

    }
</pre>

and in our Concrete folder comes our context classes

EFDContext.cs
<pre>
public class EFDbContext:DbContext
    {
        public virtual IDbSet<Url> Urls { get; set; }
    }
</pre>

EFUrlRepository.cs
<pre>
 public class EFUrlRepository : IUrlsRepository
    {
        public EFDbContext context = new EFDbContext();
        private Security security = new Security();
        public IQueryable<Url> Urls
        {
            get { return context.Urls; }
        }

        public bool AddUrl(Url url)
        {
            //Our add/save url initiative
        }
    }
</pre>

The classes flow above ensure the inversion of any depency that might ensued as explain in the Dependency Inversion Principle of the SOLID Principle.

- <b>UrlShortener.WebUI</b>

  The important code flow i want to emphasize on here is our ninject dependecny injector.
  
  In our Infrastructure folder lies our ninjectControllerFactory class.
  
  <pre>
  public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectkernel;

        public NinjectControllerFactory()
        {
        //create object of the ninject kernel
            ninjectkernel = new StandardKernel();
            //We then add our bindings that explain how our controllers call
            //should be handled
            AddBindings();
        }
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            //Each page request comes here first, the controller name is then return as requested
            return controllerType == null
                ? null
                : (IController)ninjectkernel.Get(controllerType);
        }
        
        private void AddBindings()
        {
           ninjectkernel.Bind<IUrlsRepository>().To<EFUrlRepository>();
        }
  }
  
  
</pre>
  
The GetControllerInstance method above serve the user with the controller requested by the user . Example when http://domainname.com/ControllerName .  The ControllerName is passed to the method which inturn searc through the available controllers for the said controller. If found return , otherwise throws not found exception . The AddBindings method above execured in our constructor ensure that the AdddBindings() is executed which in turn inject the IurlsRepository depency to each controller's construct when called. This can be observe in our controller constructor below

<pre>
 public UrlShortenerController(IUrlsRepository repo)
        {
            //Inject Dependency.
            repository = repo;
            
        }
</pre>

# Contribution

The project is solely developed and maintained by Nuru Salihu Abdullahi
