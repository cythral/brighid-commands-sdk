using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

internal class AutoAttribute : AutoDataAttribute
{
    public AutoAttribute()
        : base(Create)
    {
    }

    public static IFixture Create()
    {
        var fixture = new Fixture();
        fixture.Inject(new CancellationToken(false));
        fixture.Register(() => SyntaxFactory.Parameter(SyntaxFactory.ParseToken("parameter" + Guid.NewGuid().ToString().Replace("-", string.Empty))));
        fixture.Register(() => SyntaxFactory.ClassDeclaration(Guid.NewGuid().ToString()));
        fixture.Register(() => SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(Guid.NewGuid().ToString().Replace('-', '.'))));
        fixture.Register(() => ImmutableArray.Create<KeyValuePair<string, TypedConstant>>());
        fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        fixture.Customizations.Add(new OptionsRelay());
        fixture.Customizations.Add(new TypeOmitter<IDictionary<string, JsonElement>>());
        fixture.Customizations.Add(new TypeOmitter<JsonConverter>());
        fixture.Customizations.Add(new TypeOmitter<MemoryStream>());
        fixture.Customizations.Add(new TypeOmitter<GeneratorExecutionContext>());
        fixture.Customizations.Add(new TypeOmitter<GeneratorExecutionContext?>());
        fixture.Customizations.Insert(-1, new TargetRelay());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }
}
