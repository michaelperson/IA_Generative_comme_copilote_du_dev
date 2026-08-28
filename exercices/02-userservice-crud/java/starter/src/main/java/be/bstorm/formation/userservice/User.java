package be.bstorm.formation.userservice;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;

/**
 * Représente un utilisateur tel qu'exposé par l'API REST externe /users.
 * L'API répond en JSON camelCase ; les noms de champs Java (id, name, email)
 * correspondent déjà telles quelles aux clés JSON attendues.
 */
public record User(
        @JsonProperty("id") Integer id,
        @JsonProperty("name") String name,
        @JsonProperty("email") String email) {

    @JsonCreator
    public User {
        // Constructeur canonique explicite pour que Jackson puisse désérialiser
        // ce record immuable directement depuis le JSON de l'API.
    }
}
