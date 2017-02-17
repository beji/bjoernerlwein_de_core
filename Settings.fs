namespace BjoernerlweinDe.Core

module Settings =
    type Environment = 
        | Development
        | Production

    let CurrentEnvironment =
        match System.Environment.GetEnvironmentVariable "ASPNETCORE_ENVIRONMENT" with
        | "Production" -> Production
        | _ -> Development