using Dynatron.Domain;
using Dynatron.Shared;

namespace Dynatron.Api.IntegrationTests
{
    internal class TestData : ISeedData<Customer>
    {
        public IEnumerable<Customer> GetData()
        {
            yield return new Customer("Gilbert", "Gleason", "GilbertGleason@yahoo.com");
            yield return new Customer("Jessie", "Sweet", "JessieSweet@me.com");
            yield return new Customer("Alberta", "Gallagher", "AlbertaGallagher@hotmail.com");
            yield return new Customer("Mickey", "Hyde", "MickeyHyde@yahoo.com");
            yield return new Customer("Traci", "Stout", "TraciStout@email.com");
            yield return new Customer("Mabel", "Cordova", "MabelCordova@gmail.com");
            yield return new Customer("Van", "Maher", "VanMaher@email.com");
            yield return new Customer("Jan", "Carney", "JanCarney@yahoo.com");
            yield return new Customer("Alyssa", "Leblanc", "AlyssaLeblanc@me.com");
            yield return new Customer("Robyn", "Lucas", "RobynLucas@email.com");
        }
    }
}
