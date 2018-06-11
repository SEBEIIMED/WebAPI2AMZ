Webapi2 Rest avec authentification basique et authentification avec le principe d’autorisation Amazone.

Technologies ; WebApi2, nunit, visual studio 2017, .Net framework 4.6.

La solution contient trois projets : 

            1- RestAPI : Projet Webapi2 (implémente une authentification simple + L’accès à un ressource avec le principe d’autorisation Amazon).
            2- RestAPI.Test : Projet de test unitaire pour le provider d'autorisation Amazon (l'ensemble des cas possibles)
            3- ResAPI.Security : Implémente le principe d'autorisation Amazon (voir https://docs.aws.amazon.com/AmazonS3/latest/dev/RESTAuthentication.html).

