using DocumentComposition.Domain.Binder;
using DocumentComposition.Infrastructure.Data;

namespace DocumentComposition.Infrastructure.Binders;

internal class BinderRepository(DocumentCompositionDbContext dbContext) : EfCoreBaseAggregateRepository<Binder, BinderId>(dbContext), IBinderRepository;
