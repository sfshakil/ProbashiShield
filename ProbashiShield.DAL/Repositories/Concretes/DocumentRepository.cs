using ProbashiShield.DAL.Repositories.Base;
using ProbashiShield.DAL.Repositories.Contracts;
using ProbashiShield.Database.DBContexts;
using ProbashiShield.Database.DBEntities;

namespace ProbashiShield.DAL.Repositories.Concretes
{
    public class DocumentRepository : GenericRepository<Document, MasterDbContext>, IDocumentRepository
    {
        public DocumentRepository(MasterDbContext context) : base(context)
        {
        }
    }
}
