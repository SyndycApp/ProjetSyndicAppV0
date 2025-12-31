namespace SyndicApp.Domain.Enums.Assemblees;

public enum TypeAssemblee
{
    Ordinaire = 1,
    Extraordinaire = 2
}

public enum StatutAssemblee
{
    Brouillon = 1,
    Publiee = 2,
    EnCours = 3,
    Cloturee = 4,
    Annulee = 5
}

public enum ChoixVote
{
    Pour = 1,
    Contre = 2,
    Abstention = 3
}

public enum StatutResolution
{
    EnAttente = 1,
    Adoptee = 2,
    Rejetee = 3
}


public enum TypePresence
{
    Physique = 1,
    EnLigne = 2,
    Representee = 3
}

public enum TypeMajorite
{
    Simple = 1,
    Absolue = 2,
    Unanimite = 3,
    Personnalisee = 4
}
