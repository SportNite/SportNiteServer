using HotChocolate.Data.Sorting;
using NetTopologySuite.Geometries;

namespace SportNiteServer.Entities;

public class GeometryOverlaySortInput : SortInputType<GeometryOverlay>
{
    protected override void Configure(ISortInputTypeDescriptor<GeometryOverlay> descriptor)
    {
        descriptor.BindFieldsExplicitly();
    }
}