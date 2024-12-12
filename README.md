# Patte D'Oie

Ce projet est une application web permettant de jouer en ligne de façon interactive avec des joueurs du monde entier.
Les jeux disponibles actuellement sont : 
- Scattergories : Petit bac
- Speedtyping : Concours de vitesse de frappe

## Installation

Après avoir cloné le projet, plusieurs méthodes d'installations sont possibles.
Depuis Visual Studio, il faut : 
- Sélectionner le projet migrations
- Exécuter le lancement "MigrationsApply" (attendre que le conteneur migrations se soit fermé)
- Sélectionner le projet PatteDoie
- Exécuter le lancement "Docker Compose"

Il est également possible d'utiliser le docker compose complet contenant un environnement de développement avec applications des migrations automatiques : 
```bash
docker-compose -f compose.migration.yml up
```

## Utilisation

Le projet est accessible sur le port 8080 en http. Si Visual Studio est utilisé, le port 8081 est également ouvert pour pouvoir utiliser l'HTTPS si besoin.