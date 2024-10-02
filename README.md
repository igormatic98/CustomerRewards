# CustomerRewards

## Opis Projekta
**CustomerRewards** je aplikacija za dodjelu nagrada najvjernijim kupcima. Projekat omogućava jednoj kompaniji da kreira više kampanja, koje zatim
vode različiti agenti. U okviru tih kampanja, agenti dodjeljuju kupcima bonove određene vrijednosti koji mogu biti iskorišteni za kupovinu više artikala do ukupne vrijednosti bona.

## Konfiguracija Prije Pokretanja
- Prije nego što pokrenete aplikaciju, potrebno je podesiti `ConnectionString` u datoteci `appsettings.json` kako bi se aplikacija mogla povezati s bazom podataka.

## Seed Podaci
Prilikom prvog pokretanja aplikacije, seed podaci automatski kreiraju sljedeće:
- **Role**: `Direktor`, `Agent`, `Prodavač`
- **Korisnici**: Po jedan korisnik za svaku od navedenih rola (Direktor, Agent, Prodavač)
- **Kompanija** i **Početna Kampanja**: Automatski se kreira jedna kompanija i početna kampanja.

## API Endpoints

1. **Login API**
   - `https://localhost:44300/api/Authentication`
   - Koristi se za prijavu korisnika.

2. **Dodjela Bona Kupcu (rola: Agent)**
   - `https://localhost:44300/api/CustomerReward/1/1000`
   - Primjer API-ja za dodjelu bona nekom kupcu. Prvi parametar predstavlja ID kupca, dok drugi parametar predstavlja iznos bona.

3. **Potvrda Korištenja Bona (rola: Kupac)**
   - `https://localhost:44300/api/UsedReward/1/1000`
   - Primjer API-ja za potvrdu korištenja bona od strane kupca.
  
4. **Ručno Pokretanje Generisanja Fajla**
   - `https://localhost:44300/hangfire`
   - Generisanje fajla se može ručno pokrenuti na ovoj adresi.

5. **Dohvatanje Generisanog Fajla o Završetku Kampanje (role: Agent, Direktor)**
   - `https://localhost:44300/api/CampaignDocument/1`
   - API za dobijanje generisanog fajla sa kupcima koji su iskoristili bon u nedavno završenoj kampanji.


## Napomena
- Projekat je testiran na uzorku od 30 kupaca iz **findPerson** servisa.
