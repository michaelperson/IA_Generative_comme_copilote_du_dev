package be.bstorm.formation.userservice;

import org.junit.jupiter.api.Test;

import java.net.URI;
import java.net.http.HttpClient;

import static org.junit.jupiter.api.Assertions.assertThrows;

/**
 * Ces tests échouent tant que UserServiceImpl n'est pas complété (EXERCICE).
 * Ils servent de point de départ ; complétez-les et ajoutez les cas décrits
 * dans le README (succès CRUD, code non-2xx, timeout) au fur et à mesure de
 * votre implémentation.
 */
class UserServiceImplTest {

    @Test
    void getByIdAsync_nEstPasEncoreImplemente() {
        UserServiceImpl service = new UserServiceImpl(HttpClient.newHttpClient(), URI.create("http://localhost:0"));
        // TODO: une fois getByIdAsync implémentée, remplacez ce test par de vrais cas
        // (succès, 404 -> UserServiceException, timeout -> UserServiceTimeoutException).
        assertThrows(UnsupportedOperationException.class, () -> service.getByIdAsync("1").join());
    }
}
