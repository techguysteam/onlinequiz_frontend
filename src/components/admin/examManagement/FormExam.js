import React from 'react';
import PropTypes from 'prop-types';
import Header from '../header/Header';
import AppNavbar from '../../common/navbar/AppNavbar';
import { Modal } from 'react-bootstrap';
import axios from 'axios';
import { endPointRoot } from '../../../App';
import styled from 'styled-components';

const Wrapper = styled.div`
    .modalQuestionBank .modal-dialog, .modal-90w{
        max-width: 95%!important;
    }

    .modal-90w{
        max-width: 95%!important;
    }

    .modalQuestionBank .card.card-question{
        font-size: 0.8em;
        padding: 5px;
        border: 2px solid #ccc;
        cursor: pointer;
    }

    .modalQuestionBank .card.card-question:hover{
        background-color: rgba(0,0,0,0.1);
    }

    .modalQuestion .modal-content{
        background-color: rgba(200,200,200);
    }

    .checkbox-select-question{
        width: 20px;
        height: 20px;
        float: right;
    }

    .img-question-card{
        width: 90%;
        max-height: 100px;
        /* margin-left: 10px; */
        padding-left: 10px;
    }

    .selected-questions{
        border-bottom: 2px solid #ccc;
    }

    .selected-card-question{
        border:  2px solid green!important;
    }

    .modal{
        overflow-y:auto;
    }
`;

class FormExam extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            title: '',
            showModal: false,
            exam: { name: '', duration: 90, totalQuestion: 30, openDate: '', openTime: '', year: new Date().getFullYear()  }
        };
    }

    componentDidMount = () => {
        let id = this.props.match.params.examId;
        let title = 'Tạo đề thi';
        if(id){
            title = 'Cập nhật đề thi'
            this.getExam(id);
        }
        this.setState({ title })
    }

    getTitle = () => {
        
    }

    getExam = async (id) => { 
        const url = endPointRoot + '/api/exam'
        const res = await axios.get(url, { examId: id });
        const { question } = res.data;
        this.setState({ question });
        console.log(question);
    }

    onChange = e => {
        const { name, value } = e.target;
        let exam = Object.assign({}, this.state.exam);
        exam[name] = value;
        this.setState({ exam });
    }

    submit = e => {
        e.preventDefault();
        const { exam } = this.state;
        console.log(exam);
        exam.openTime = exam.openDate + ' ' + exam.openTime;
        delete exam.openDate;
        const url = endPointRoot + '/api/exam';
        axios.post(url, exam)
        .then(res => {
            console.log(res)
        })
        .catch(err => console.log(err));
    }

    handleClose = () => {
        this.setState({ showModal: false });
    }

    showQuestionsBank = () => {
        this.setState({ showModal: true });
    }

    render() {
        const { name, duration, totalQuestion, openDate, openTime, year } = this.state.exam;
        return (
            <Wrapper>
                <AppNavbar/>
                <Header title={this.state.title} />
                    <section style={{ marginBottom: '70px' }}>
                        <div className="container">
                            <div className="filter mt-5">
                                <div className="row">
                                    <div className="col-md-6 col-sm-8 mx-auto">
                                        <div className="card">
                                            <div className="card-body">
                                                <form onSubmit={this.submit}>
                                                    <div className="form-group">
                                                        <strong>Tên đề thi</strong>
                                                        <input 
                                                            type="text" 
                                                            placeholder="Nhập tên đề thi" 
                                                            className="form-control"
                                                            value={name}
                                                            name="name"
                                                            onChange={this.onChange}
                                                        />
                                                    </div>
                                                    <div className="form-group">
                                                        <strong>Năm thi</strong>
                                                        <input 
                                                            type="text" 
                                                            placeholder="Nhập năm thi" 
                                                            className="form-control"
                                                            value={year}
                                                            name="year"
                                                            onChange={this.onChange}
                                                        />
                                                    </div>
                                                    <div className="form-group">
                                                        <strong>Số câu hỏi</strong>
                                                        <input 
                                                            type="text" 
                                                            placeholder="Nhap số lượng câu hỏi" 
                                                            className="form-control"
                                                            value={totalQuestion}
                                                            name="totalQuestion"
                                                            onChange={this.onChange}
                                                        />
                                                    </div>
                                                    <div className="form-group">
                                                        <strong>Thời gian làm bài (phút)</strong>
                                                        <input 
                                                            type="text" 
                                                            placeholder="Nhập thòi gian làm bài" 
                                                            className="form-control"
                                                            value={duration}
                                                            name="duration"
                                                            onChange={this.onChange}
                                                        />
                                                    </div>
                                                    <div className="form-group">
                                                        <strong>Thời mở (phút)</strong>
                                                        <input 
                                                            type="date" 
                                                            placeholder="Nhập ngầy" 
                                                            className="form-control"
                                                            value={openDate}
                                                            name="openDate"
                                                            onChange={this.onChange}
                                                        />
                                                        <input 
                                                            type="time" 
                                                            placeholder="Nhập giờ" 
                                                            className="form-control"
                                                            value={openTime}
                                                            name="openTime"
                                                            onChange={this.onChange}
                                                        />
                                                    </div>
                                                    <button className="btn btn-outline-success float-right ml-2">Lưu</button>
                                                    <button type="button" id="btnShowModalQuestionBank" className="btn btn-outline-warning float-right" onClick={this.showQuestionsBank}>Thêm câu hỏi</button>
                                                </form>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                    <section>
                    <Modal dialogClassName="modal-90w" show={this.state.showModal} onHide={this.handleClose}>
                        <Modal.Header closeButton>
                            <Modal.Title>Ngân hàng câu hỏi</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <div class="selected-questions">
                                <h4 class="text-center font-italic">Những câu hỏi đã chọn</h4>
                                <div class="row mb-3">
                                    <div class="col-lg-3 col-md-4 col-sm-6 px-1 mb-2">
                                        <div class="card card-body card-question selected-card-question">
                                            <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Temporibus, ea!...</p>
                                            <div>
                                                <input type="checkbox" checked class="checkbox-select-question float-right"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                                <div class="question-bank mt-5">
                                <h4 class="text-center font-italic">Các câu hỏi trong ngân hàng</h4>
                                <div class="row">
                                    <div class="col-lg-3 col-md-4 col-sm-6 px-1 mb-2">
                                        <div class="card card-body card-question selected-card-question">
                                            <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Temporibus, ea!...</p>
                                            <div>
                                                <input type="checkbox" checked class="checkbox-select-question float-right"/>
                                            </div>
                                        </div>
                                    </div>
                                
                                    <div class="col-lg-3 col-md-4 col-sm-6 px-1 mb-2">
                                        <div class="card card-body card-question">
                                            <div class="row">
                                                <div class="col-5 px-1">
                                                    <img src="../../images/cat.png" alt="" class="img-question-card"/>
                                                </div>
                                                <div class="col-7 px-1">
                                                    <p>Lorem ipsum dolor sit amet consectetur, adipisicing elit. Temporibus, ea!...</p>
                                                </div>
                                            </div>
                                            <div>
                                                <input type="checkbox" class="checkbox-select-question float-right"/>
                                            </div>
                                        </div>
                                    </div>
                                
                            </div>
                                {/* <div class="row text-center mt-3">
                                    <div class="col-sm-4 mx-auto">
                                            <button class="btn btn-block btn-outline-warning" id="btnLoadMoreQuestions">Hiển thị thêm câu hỏi khác</button>
                                    </div>
                                </div> */}
                                </div>
                        </Modal.Body>
                            <Modal.Footer>
                                <button className="btn btn-outline-primary" onClick={this.handleClose}>
                                    Close
                                </button>
                            </Modal.Footer>
                    </Modal>
                    </section>
            </Wrapper>
        );
    }
}

FormExam.propTypes = {};

export default FormExam;
