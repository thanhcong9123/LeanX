document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('exerciseForm');
    const cancelBtn = document.getElementById('cancelBtn');
    const successMessage = document.getElementById('successMessage');
    const errorMessage = document.getElementById('errorMessage');
    const errorText = document.getElementById('errorText');
    const questionsContainer = document.getElementById('questionsContainer');
    const addQuestionBtn = document.getElementById('addQuestionBtn');
    const fileName = document.getElementById('fileName');

    let questionCounter = 0;
    let questions = [];

    // Expose để generateAIQuestions có thể dùng
    window.questionsContainer = questionsContainer;
    window.questions = questions;
    window.questionCounter = questionCounter;

    // Add question functionality
    addQuestionBtn.addEventListener('click', function () {
        addQuestion();
    });

    function addQuestion() {
        questionCounter++;
        window.questionCounter = questionCounter; // Sync global
        
        const questionData = {
            id: questionCounter,
            questionId: 0,
            questionText: '',
            answers: []
        };

        questions.push(questionData);

        const questionHtml = `
            <div class="question-item" data-question-id="${questionCounter}">
                <div class="question-header">
                    <div class="question-number">Q${questionCounter}</div>
                    <button type="button" class="remove-question-btn" onclick="removeQuestion(${questionCounter})">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
                
                <input type="hidden" name="QuestionRequest[${questionCounter - 1}].QuestionId" value="0">
                <input type="text" name="QuestionRequest[${questionCounter - 1}].QuestionText" 
                       class="question-text-input" placeholder="Nhập câu hỏi..." 
                       onchange="updateQuestionText(${questionCounter}, this.value)" required>
                
                <div class="answers-container">
                    <div class="answers-header">
                        <span class="answers-title">Đáp án (chọn đáp án đúng)</span>
                        <button type="button" class="add-answer-btn" onclick="addAnswerGlobal(${questionCounter})">
                            <i class="fas fa-plus"></i>
                            Thêm đáp án
                        </button>
                    </div>
                    <div class="answers-list" id="answers_${questionCounter}">
                    </div>
                </div>
            </div>
        `;

        const emptyState = questionsContainer.querySelector('.empty-questions');
        if (emptyState) {
            emptyState.remove();
        }

        questionsContainer.insertAdjacentHTML('beforeend', questionHtml);

        // Add default 2 answers
        addAnswer(questionCounter);
        addAnswer(questionCounter);
    }

    function addAnswer(questionId) {
        const question = questions.find(q => q.id === questionId);
        if (!question) return;

        const answerIndex = question.answers.length;

        const answerData = {
            answerId: 0,
            answerText: '',
            isCorrect: false
        };

        question.answers.push(answerData);

        const answerHtml = `
            <div class="answer-item" data-answer-index="${answerIndex}">
                <input type="hidden" name="QuestionRequest[${questionId - 1}].Answers[${answerIndex}].AnswerId" value="0">
                <div class="answer-checkbox" onclick="toggleCorrectAnswer(${questionId}, ${answerIndex})"></div>
                <input type="hidden" name="QuestionRequest[${questionId - 1}].Answers[${answerIndex}].IsCorrect" 
                       value="false" class="is-correct-input">
                <input type="text" name="QuestionRequest[${questionId - 1}].Answers[${answerIndex}].AnswerText" 
                       class="answer-input" placeholder="Nhập đáp án..." 
                       onchange="updateAnswerText(${questionId}, ${answerIndex}, this.value)" required>
                <button type="button" class="remove-answer-btn" onclick="removeAnswer(${questionId}, ${answerIndex})">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        `;

        const answersList = document.getElementById(`answers_${questionId}`);
        answersList.insertAdjacentHTML('beforeend', answerHtml);
    }

    // === GLOBAL FUNCTIONS ===
    window.addAnswerGlobal = function(questionId) {
        addAnswer(questionId);
    };

    window.removeQuestion = function (questionId) {
        const questionElement = document.querySelector(`[data-question-id="${questionId}"]`);
        if (questionElement) {
            questionElement.remove();
            questions = questions.filter(q => q.id !== questionId);
            window.questions = questions;

            if (questions.length === 0) {
                questionsContainer.innerHTML = `
                    <div class="empty-questions">
                        <i class="fas fa-question-circle"></i>
                        <p>Chưa có câu hỏi nào. Nhấn "Thêm câu hỏi" để bắt đầu.</p>
                    </div>
                `;
            }
            renumberQuestions();
        }
    };

    window.removeAnswer = function (questionId, answerIndex) {
        const question = questions.find(q => q.id === questionId);
        if (!question) return;

        if (question.answers.length <= 2) {
            showError('Mỗi câu hỏi phải có ít nhất 2 đáp án!');
            return;
        }

        const answerElement = document.querySelector(`[data-question-id="${questionId}"] [data-answer-index="${answerIndex}"]`);
        if (answerElement) {
            answerElement.remove();
            question.answers.splice(answerIndex, 1);
            renumberAnswers(questionId);
        }
    };

    window.toggleCorrectAnswer = function (questionId, answerIndex) {
        const question = questions.find(q => q.id === questionId);
        if (!question) return;

        // Reset all answers
        question.answers.forEach((answer, index) => {
            answer.isCorrect = false;
            const checkbox = document.querySelector(`[data-question-id="${questionId}"] [data-answer-index="${index}"] .answer-checkbox`);
            const hiddenInput = document.querySelector(`[data-question-id="${questionId}"] [data-answer-index="${index}"] .is-correct-input`);
            if (checkbox) checkbox.classList.remove('checked');
            if (hiddenInput) hiddenInput.value = 'false';
        });

        // Set selected
        question.answers[answerIndex].isCorrect = true;
        const selectedCheckbox = document.querySelector(`[data-question-id="${questionId}"] [data-answer-index="${answerIndex}"] .answer-checkbox`);
        const selectedHiddenInput = document.querySelector(`[data-question-id="${questionId}"] [data-answer-index="${answerIndex}"] .is-correct-input`);
        if (selectedCheckbox) selectedCheckbox.classList.add('checked');
        if (selectedHiddenInput) selectedHiddenInput.value = 'true';
    };

    window.updateQuestionText = function (questionId, text) {
        const question = questions.find(q => q.id === questionId);
        if (question) {
            question.questionText = text;
        }
    };

    window.updateAnswerText = function (questionId, answerIndex, text) {
        const question = questions.find(q => q.id === questionId);
        if (question && question.answers[answerIndex]) {
            question.answers[answerIndex].answerText = text;
        }
    };

    function renumberQuestions() {
        const questionElements = document.querySelectorAll('.question-item');
        questionElements.forEach((element, index) => {
            const questionNumber = element.querySelector('.question-number');
            if (questionNumber) {
                questionNumber.textContent = `Q${index + 1}`;
            }

            const inputs = element.querySelectorAll('input, select, textarea');
            inputs.forEach(input => {
                if (input.name && input.name.includes('QuestionRequest[')) {
                    input.name = input.name.replace(/QuestionRequest\[\d+\]/, `QuestionRequest[${index}]`);
                }
            });
        });
    }

    function renumberAnswers(questionId) {
        const answerElements = document.querySelectorAll(`[data-question-id="${questionId}"] .answer-item`);
        answerElements.forEach((element, index) => {
            element.setAttribute('data-answer-index', index);
            const inputs = element.querySelectorAll('input');
            inputs.forEach(input => {
                if (input.name && input.name.includes('.Answers[')) {
                    input.name = input.name.replace(/\.Answers\[\d+\]/, `.Answers[${index}]`);
                }
            });
        });
    }

    function validateForm() {
        if (questions.length === 0) {
            showError('Vui lòng thêm ít nhất một câu hỏi!');
            return false;
        }

        for (let question of questions) {
            if (!question.questionText.trim()) {
                showError('Vui lòng nhập nội dung cho tất cả câu hỏi!');
                return false;
            }

            if (question.answers.length < 2) {
                showError('Mỗi câu hỏi phải có ít nhất 2 đáp án!');
                return false;
            }

            const hasCorrectAnswer = question.answers.some(answer => answer.isCorrect);
            if (!hasCorrectAnswer) {
                showError('Mỗi câu hỏi phải có ít nhất một đáp án đúng!');
                return false;
            }

            for (let answer of question.answers) {
                if (!answer.answerText.trim()) {
                    showError('Vui lòng nhập nội dung cho tất cả đáp án!');
                    return false;
                }
            }
        }
        return true;
    }

    form.addEventListener('submit', function (e) {
        syncLatestValues();
        if (!validateForm()) {
            e.preventDefault();
            return false;
        }
    });

    function syncLatestValues() {
        document.querySelectorAll('.question-item').forEach(qEl => {
            const qId = parseInt(qEl.getAttribute('data-question-id'));
            const qObj = questions.find(q => q.id === qId);
            if (qObj) {
                const qInput = qEl.querySelector('input.question-text-input');
                if (qInput) qObj.questionText = qInput.value || '';
                
                qEl.querySelectorAll('.answer-item').forEach(aEl => {
                    const idx = parseInt(aEl.getAttribute('data-answer-index'));
                    const ansInput = aEl.querySelector('input.answer-input');
                    const hiddenCorrect = aEl.querySelector('input.is-correct-input');
                    if (qObj.answers[idx]) {
                        if (ansInput) qObj.answers[idx].answerText = ansInput.value || '';
                        if (hiddenCorrect) qObj.answers[idx].isCorrect = hiddenCorrect.value === 'true';
                    }
                });
            }
        });
    }

    function showSuccess(message) {
        successMessage.querySelector('span').textContent = message;
        successMessage.style.display = 'flex';
        errorMessage.style.display = 'none';
        setTimeout(() => successMessage.style.display = 'none', 5000);
    }

    function showError(message) {
        errorText.textContent = message;
        errorMessage.style.display = 'flex';
        successMessage.style.display = 'none';
        setTimeout(() => errorMessage.style.display = 'none', 5000);
    }

    cancelBtn.addEventListener('click', function () {
        const hasContent = form.querySelector('input[type="text"]').value ||
            form.querySelector('textarea').value ||
            questions.length > 0;

        if (hasContent) {
            const confirmDialog = document.createElement('div');
            confirmDialog.style.cssText = `
                position: fixed; top: 0; left: 0; right: 0; bottom: 0;
                background: rgba(0, 0, 0, 0.5); display: flex;
                align-items: center; justify-content: center; z-index: 1000;
            `;

            confirmDialog.innerHTML = `
                <div style="background: white; padding: 30px; border-radius: 20px; max-width: 400px; text-align: center;">
                    <i class="fas fa-exclamation-triangle" style="font-size: 48px; color: #f59e0b; margin-bottom: 20px;"></i>
                    <h3 style="margin: 0 0 15px 0; color: #2d3748;">Xác nhận hủy bỏ</h3>
                    <p style="margin: 0 0 25px 0; color: #64748b;">Bạn có chắc chắn muốn hủy bỏ? Tất cả dữ liệu đã nhập sẽ bị mất.</p>
                    <div style="display: flex; gap: 15px;">
                        <button id="confirmCancel" style="flex: 1; padding: 12px; background: #ef4444; color: white; border: none; border-radius: 10px; font-weight: 600; cursor: pointer;">Xác nhận</button>
                        <button id="keepEditing" style="flex: 1; padding: 12px; background: #f8fafc; color: #64748b; border: 2px solid #e2e8f0; border-radius: 10px; font-weight: 600; cursor: pointer;">Tiếp tục</button>
                    </div>
                </div>
            `;

            document.body.appendChild(confirmDialog);

            document.getElementById('confirmCancel').addEventListener('click', function () {
                resetForm();
                document.body.removeChild(confirmDialog);
            });

            document.getElementById('keepEditing').addEventListener('click', function () {
                document.body.removeChild(confirmDialog);
            });
        } else {
            resetForm();
        }
    });

    function resetForm() {
        form.reset();
        questions = [];
        window.questions = questions;
        questionCounter = 0;
        window.questionCounter = questionCounter;
        questionsContainer.innerHTML = `
            <div class="empty-questions">
                <i class="fas fa-question-circle"></i>
                <p>Chưa có câu hỏi nào. Nhấn "Thêm câu hỏi" để bắt đầu.</p>
            </div>
        `;
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    // === AI GENERATION ===
    document.getElementById('generateAIQuestionsBtn').addEventListener('click', function () {
        document.getElementById('aiModal').style.display = 'flex';
        const currentTitle = document.getElementById('title').value;
        if (currentTitle) document.getElementById('aiTitle').value = currentTitle;
        
        const currentDescribe = document.getElementById('describe').value;
        if (currentDescribe) document.getElementById('aiContent').value = currentDescribe;
    });

    window.closeAIModal = function() {
        document.getElementById('aiModal').style.display = 'none';
    };

    window.generateAIQuestions = async function() {
        const title = document.getElementById('aiTitle').value;
        const content = document.getElementById('aiContent').value;
        const numberOfQuestions = parseInt(document.getElementById('aiNumberOfQuestions').value);

        if (!title || !content) {
            alert('Vui lòng nhập đầy đủ thông tin!');
            return;
        }

        const loadingMsg = document.getElementById('aiLoadingMessage');
        loadingMsg.style.display = 'block';

        try {
            const response = await fetch('/Exercise/GenerateAIQuestions', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({
                    title: title,
                    content: content,
                    numberOfQuestions: numberOfQuestions
                })
            });

            if (!response.ok) {
                const error = await response.json();
                throw new Error(error.error || 'Có lỗi xảy ra');
            }

            const data = await response.json();

            // Reset
            questionsContainer.innerHTML = '';
            questions = [];
            questionCounter = 0;

            // Add AI questions
            data.questions.forEach((q) => {
                addQuestion(); // Tạo câu hỏi mới với 2 đáp án mặc định
                
                const currentQuestionId = questionCounter; // ID của câu hỏi vừa tạo
                const currentQuestion = questions[questions.length - 1]; // Câu hỏi vừa tạo

                // Update question text
                const questionInput = document.querySelector(`[data-question-id="${currentQuestionId}"] input.question-text-input`);
                if (questionInput) {
                    questionInput.value = q.questionText;
                    currentQuestion.questionText = q.questionText;
                }

                // Update answers
                q.answers.forEach((answer, answerIndex) => {
                    // Nếu đã có đáp án (2 đáp án mặc định), update
                    if (answerIndex < currentQuestion.answers.length) {
                        const answerTextInput = document.querySelector(`[data-question-id="${currentQuestionId}"] [data-answer-index="${answerIndex}"] input.answer-input`);
                        if (answerTextInput) {
                            answerTextInput.value = answer.answerText;
                            currentQuestion.answers[answerIndex].answerText = answer.answerText;
                            currentQuestion.answers[answerIndex].isCorrect = answer.isCorrect;
                        }
                    } else {
                        // Thêm đáp án mới
                        addAnswer(currentQuestionId);
                        const newAnswerIndex = currentQuestion.answers.length - 1;
                        const answerTextInput = document.querySelector(`[data-question-id="${currentQuestionId}"] [data-answer-index="${newAnswerIndex}"] input.answer-input`);
                        if (answerTextInput) {
                            answerTextInput.value = answer.answerText;
                            currentQuestion.answers[newAnswerIndex].answerText = answer.answerText;
                            currentQuestion.answers[newAnswerIndex].isCorrect = answer.isCorrect;
                        }
                    }

                    // Update correct answer checkbox
                    if (answer.isCorrect) {
                        const checkbox = document.querySelector(`[data-question-id="${currentQuestionId}"] [data-answer-index="${answerIndex}"] .answer-checkbox`);
                        const hiddenInput = document.querySelector(`[data-question-id="${currentQuestionId}"] [data-answer-index="${answerIndex}"] .is-correct-input`);
                        if (checkbox) checkbox.classList.add('checked');
                        if (hiddenInput) hiddenInput.value = 'true';
                    }
                });
            });

            window.questions = questions;
            window.questionCounter = questionCounter;

            closeAIModal();
            alert(`Đã tạo thành công ${data.questions.length} câu hỏi!`);

        } catch (error) {
            console.error('Error:', error);
            alert('Có lỗi xảy ra: ' + error.message);
        } finally {
            loadingMsg.style.display = 'none';
        }
    };

    // Add first question
    addQuestion();
});