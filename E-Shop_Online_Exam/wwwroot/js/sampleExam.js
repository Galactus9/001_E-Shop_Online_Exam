
﻿function createQuestionTabButtons(numberOfQuestions) {
    let list = document.querySelector("#questionsButtons");
    console.log(list);
    for (i = 2; i <= numberOfQuestions; i++) {
        let questionTab = document.createElement("li");
        questionTab.classList.add("nav-item");
        let questionTabLink = document.createElement("a");
        questionTabLink.classList.add("nav-link");
        questionTabLink.setAttribute("data-bs-toggle", "tab");
        questionTabLink.setAttribute("href", `#form${i}`);
        questionTabLink.innerHTML = `Question ${i}`;
        questionTab.appendChild(questionTabLink);
        console.log(questionTab);
        list.appendChild(questionTab);
    }
}
//function createQuestionBody
createQuestionTabButtons(100);
console.log("test-final");