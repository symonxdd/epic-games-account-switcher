namespace AccountSwitcher.Resources;

public static class CapybaraSentences {

  private static readonly Random Random = new();

  private static readonly List<string> Sentences = new() {
    "Jestem słodką, biedną kapibarą, co zgubiła swoje ulubione liście sałaty.",
    "Kapibara na basenie: poziom relaksu – ekspert.",
    "Woda to moje królestwo, a ja jestem królem chillowania.",
    "Siedzę sobie w wodzie i myślę: nic nie muszę, wszystko mogę.",
    "Może i jestem zwykłą kapibarą, ale w tym momencie jestem gwiazdą tego spa.",
    "Basen to życie, a życie to basen. Prosty wybór.",
    "Gdzie ja, tam chill. Kapibara style.",
    "Kąpiel to sztuka, którą kapibary opanowały do perfekcji.",
    "Może podacie mi parasolkę i drinka? Proszę, zasłużyłem.",
    "Bąbelki w wodzie? Jeszcze nie, ale to marzenie każdej kapibary.",
    "Czasem siedzę w wodzie i się zastanawiam: dlaczego jestem taki uroczy?",
    "Kapibara w wodzie to synonim czystej relaksacji.",
    "Prawdziwy spokój to ja, w wodzie, sam na sam ze sobą.",
    "Nie ma nic lepszego niż ciepła woda i spokojne myśli.",
    "Dziś chilluję, jutro pewnie też. Takie życie kapibary.",
    "Zimna woda, gorące serce – taki właśnie jestem.",
    "Ktoś mówił, że kapibary nie potrzebują luksusu? Spójrz na mnie teraz.",
    "Moje motto: więcej wody, mniej stresu.",
    "Siedzę w wodzie i czekam, aż ktoś poda mi marchewkę.",
    "Nie wszystko złoto, co się świeci. Ale każda kapibara w wodzie to skarb.",
    "Pływanie to dla mnie nie tylko relaks, to styl życia.",
    "Nad wodą czuję się, jakbym unosił się w chmurach.",
    "Czy to zwykła kałuża, czy basen – kapibara znajdzie tu szczęście.",
    "Relaks kapibary: zanurz się w wodzie i zapomnij o całym świecie.",
    "Basen dla kapibary? To nie luksus, to konieczność.",
    "Jestem królem mokrego chilloutu – ukłońcie się przede mną, proszę.",
    "Nie ważne, czy to kałuża czy ocean, każda kropla wody jest dla mnie rajem.",
    "Powiedziałem kiedyś, że woda mnie koi? No to powiem to jeszcze raz – kocha mnie!",
    "Gdy wchodzę do wody, cały świat przestaje istnieć.",
    "Kapibara w spa to stan umysłu – i serca, bo ciepło wody jest jak uścisk.",
    "Pod wodą wszyscy jesteśmy równi – ale ja mam większy luz.",
    "Nie potrzebuję jacuzzi – każdy strumień wody jest dla mnie źródłem radości.",
    "Jak wygląda relaks idealny? Spójrz na mnie i się ucz.",
    "Mówicie 'żyć jak król'? Ja mówię: żyć jak kapibara w wodzie.",
    "Zanurz się ze mną w chwili spokoju. Obiecuję, że nie pożałujesz.",
    "Mam łapki, które tylko do relaksu stworzono – i woda o tym wie.",
    "Każda fala to jak przytulenie natury. Czuję się kochany.",
    "Nie patrz na zegar – w wodzie czas płynie inaczej.",
    "Czy życie może być bardziej idealne? Wątpię, patrząc na siebie teraz.",
    "Woda jest dla mnie jak koc dla ludzi – otula mnie i uspokaja.",
    "Nie pytaj, dlaczego kapibary tak kochają wodę. Po prostu spróbuj.",
    "Szczęście jest wtedy, gdy kapibara ma wodę, a świat ma kapibarę.",
    "Mówią, że diamenty są wieczne – ale czy widzieli kapibarę w wodzie?",
    "Zanurzenie w wodzie to dla mnie jak reset – a teraz jestem w trybie zen.",
    "Kiedyś spróbuję surfingu, ale na razie wolę swoje spokojne wody.",
    "Każda kropla, która mnie dotyka, przypomina, że jestem we właściwym miejscu.",
    "Niech inni biegają, ja po prostu wejdę do wody i poczekam, aż życie zwolni.",
    "Jeśli woda mówi językiem relaksu, to ja jestem jej rodzimym mówcą.",
    "Powiedziałem kiedyś, że kocham wodę? To za mało. Ja jestem wodą.",
    "Każda chwila w wodzie to dla mnie oddech czystego szczęścia."
  };

  public static string GetRandomSentence() {
    return Sentences[Random.Next(Sentences.Count)];
  }
}
