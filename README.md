# WcfBackEndv2

När du har laddat ner repot så måste du göra detta innan du kör igång:

## 1: Installera Entity framework
- Klicka: Build -> build solution
- När bygget körs så laddas Entity Framework ned och installeras

## 2: Byt namn på databas
- om du har kört denna solution lokalt tidigare så kan det bli problem med den gamla databasen. FIX: gör bara sähär:
  - Öppna filen `Web.config`
  - titta längst ner, under taggen &lt;connectionStrings&gt; 
  - but ut `initial catalog=WcfBackEndv2e;` mot något annat namn, vad som helst (glöm inte semokolon på slutet).

## 3: Skapa en databas
- Gå till: Tools -> NuGet Package Manager -> Package Manager Console
- Skriv: Update-Database

## 4: Kör igång!
- markera filen "Service1.svc"
- Kör igång! 





