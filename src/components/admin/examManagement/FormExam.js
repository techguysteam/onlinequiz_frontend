import React from 'react';
import PropTypes from 'prop-types';

class FormExam extends React.Component {
    constructor(props) {
        super(props);
        this.state = {};
    }

    render() {
        return (
            <React.Fragment>
                <section style={{ marginBottom: '70px' }}>
                    <div className="container">
                        <div className="filter mt-5">
                            <div className="row">
                                <div className="col-md-6 col-sm-8 mx-auto">
                                        <div className="card">
                                            <div className="card-body">
                                                <form action="">
                                                    <div className="form-group">
                                                        <strong>Tên đề thi</strong>
                                                        <input type="text" placeholder="Nhập tên đề thi" className="form-control"/>
                                                    </div>
                                                    <div className="form-group">
                                                        <strong>Năm thi</strong>
                                                        <input type="text" placeholder="Nhập năm thi" className="form-control"/>
                                                    </div>
                                                    <div className="form-group">
                                                        <strong>Số câu hỏi</strong>
                                                        <input type="text" placeholder="Nhap số lượng câu hỏi" className="form-control"/>
                                                    </div>
                                                    <div className="form-group">
                                                        <strong>Thời gian làm bài (phút)</strong>
                                                        <input type="text" placeholder="Nhập thòi gian làm bài" className="form-control"/>
                                                    </div>
                                                    <button className="btn btn-outline-success float-right ml-2">Lưu</button>
                                                    <button type="button" id="btnShowModalQuestionBank" className="btn btn-outline-warning float-right">Thêm câu hỏi</button>
                                                </form>
                                            </div>
                                        </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                </section>
            </React.Fragment>
        );
    }
}

FormExam.propTypes = {};

export default FormExam;
