Webapi2 Rest (authentification basique et authentification avec le principe d’autorisation Amazon)

Technologies ; WebApi2, nunit, visual studio 2017, .Net framework 4.6.

La solution contient trois projets : 

            1- RestAPI : Projet Webapi2 (implémente une authentification simple + L’accès à un ressource avec le principe d’autorisation Amazon).
            2- RestAPI.Test : Projet de test unitaire pour le provider d'autorisation Amazon (couvre l'ensemble des cas possibles) et le pour le projet RestAPI.
            3- ResAPI.Security : Implémente le principe d'autorisation Amazon (voir https://docs.aws.amazon.com/AmazonS3/latest/dev/RESTAuthentication.html).

