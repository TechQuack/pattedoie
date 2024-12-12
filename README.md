# Patte D'Oie

Ce projet est une application web permettant de jouer en ligne de fa�on interactive avec des joueurs du monde entier.
Les jeux disponibles actuellement sont : 
- Scattergories : Petit bac
- Speedtyping : Concours de vitesse de frappe

## Installation

Apr�s avoir clon� le projet, plusieurs m�thodes d'installations sont possibles.
Depuis Visual Studio, il faut : 
- S�lectionner le projet migrations
- Ex�cuter le lancement "MigrationsApply" (attendre que le conteneur migrations se soit ferm�)
- S�lectionner le projet PatteDoie
- Ex�cuter le lancement "Docker Compose"

Il est �galement possible d'utiliser le docker compose complet contenant un environnement de d�veloppement avec applications des migrations automatiques : 
```bash
docker-compose -f compose.migration.yml up
```

## Utilisation

Le projet est accessible sur le port 8080 en http. Si Visual Studio est utilis�, le port 8081 est �galement ouvert pour pouvoir utiliser l'HTTPS si besoin.